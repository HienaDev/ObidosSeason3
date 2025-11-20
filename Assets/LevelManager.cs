using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using TMPro;
using System.Collections;
using static LevelManager;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Serializable]
    public struct Level
    {
        public int numberOfPeople;
        public int numberOfFaults;
        public float spawnRate;
        public float timer;

        public int badBooksNumber;
        public int badTopicsNumber;
        public int badHatsNumber;
        public bool badSinging;
        public bool badRadio;
        public bool badFootball;

        public int maximumAnomaliesActive;

        public bool specialLevel;
        public int initialChance;
        public int chanceIncrease;

        [Range(0, 1)]
        public float initialGreyscale;
    }

    [SerializeField] private FaultManager faultManager;
    [SerializeField] private GameObject uiObject;
    [SerializeField] private GameObject logosCanvas;

    [SerializeField] private Level[] levels;
    private int currentLevel;
    private int numberOfPeopleSpawned;
    private List<int> indexOfFaults;
    private List<int> availableSlots;
    private static System.Random random = new System.Random();

    private bool spawning = false;

    private float justSpawned = 0f;

    [SerializeField] private CreateTopics createTopicsScript;

    [SerializeField] private TextMeshProUGUI timerText;
    public bool isRunning = false;
    private float timeRemaining = 0f;

    [SerializeField] private CivilianBrain civilianBrainScript;

    private List<CivilianFaultType> faultTypes;

    [SerializeField] private FadeBlackScreen fadeScreen;

    public int anomaliesCount = 0;

    [SerializeField] private GameObject pointerPivot;

    [SerializeField] private TextMeshProUGUI reasonTextUI;

    //[SerializeField] private GameObject[] tabUI;

    private bool restarted = false;

    public bool revolution = false;

    [SerializeField] private ForceLoadScene forceLoadScene;

    [SerializeField]
    private GameObject musicianNPC;

    [SerializeField]
    private Material spritesGreyscaleMaterial;
    [SerializeField]
    private Material groundGreyscaleMaterial;

    private float specialLevelGreyscale = 1f;

    private bool specialDayRevolutionApproaching = false;

    [SerializeField]
    private PlayerMovement playerMov;

    private int newTopics = 0;
    private int newBooks = 0;
    private int newHats = 0;
    private int newSinging = 0;
    private int newRadio = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        availableSlots = new List<int>();
        indexOfFaults = new List<int>();
        faultTypes = new List<CivilianFaultType>();

        spritesGreyscaleMaterial.SetFloat("_GrayscaleAmount", 0);
        groundGreyscaleMaterial.SetFloat("_Greyscale", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (revolution)
            return;

        if (spawning && Time.time - justSpawned > levels[currentLevel].spawnRate )
        {

            if (levels[currentLevel].specialLevel)
            {
                createTopicsScript.IncreaseCloves();
                specialDayRevolutionApproaching = true;
                if ((anomaliesCount/ levels[currentLevel].numberOfFaults > 0.5 || (levels[currentLevel].timer - timeRemaining) / levels[currentLevel].timer > 0.5) && !specialDayRevolutionApproaching)
                {
                    Debug.Log("revolution Started");
                }

                int chanceOfspawn = UnityEngine.Random.Range(0, 100);

                if (chanceOfspawn < levels[currentLevel].initialChance)
                {
                    CivilianFaultType randomFault = (CivilianFaultType)UnityEngine.Random.Range(3, 5);
                    civilianBrainScript.CreateNewCivilian(randomFault);
                    faultManager.AddFault(specialDayRevolutionApproaching);
                }
                else
                {
                    //Debug.Log("Good Civillian Spawned");
                    civilianBrainScript.CreateNewCivilian(CivilianFaultType.None);
                }

                levels[currentLevel].initialChance += levels[currentLevel].chanceIncrease;
            }
            else
            {
                if (numberOfPeopleSpawned >= levels[currentLevel].numberOfPeople)
                {
                    spawning = false;
                }
                else
                {
                    if (indexOfFaults.Contains(numberOfPeopleSpawned))
                    {
                        indexOfFaults.Remove(numberOfPeopleSpawned);
                        // Spawn Bad Civillian
                        //Debug.Log("Bad Civillian Spawned");
                        CivilianFaultType randomFault = PriorityFaultType();
                        civilianBrainScript.CreateNewCivilian(randomFault);
                        faultManager.AddFault(false);
                    }
                    else
                    {
                        // Spawn Good Civillian
                        //Debug.Log("Good Civillian Spawned");
                        civilianBrainScript.CreateNewCivilian(CivilianFaultType.None);
                    }
                }
            }

            numberOfPeopleSpawned++;
            justSpawned = Time.time;

        }

        if(anomaliesCount > levels[currentLevel].maximumAnomaliesActive)
        {
            if(levels[currentLevel].specialLevel)
            {
                StartCoroutine(LoadRevolutionCR());
                createTopicsScript.revolutionObject.SetActive(true);
                revolution = true;
            }
            else
            {
                reasonTextUI.text = "Too many suspects active...";
                RestartDay(false);
            }

        }
        Debug.Log("index:" + indexOfFaults.Count);
        Debug.Log("anomalies:" + anomaliesCount);

        if (indexOfFaults.Count <= 0 && anomaliesCount <= 0 && isRunning)
        {
            isRunning = false;
            EndDay();
            Debug.Log("heree");
        }

        if (isRunning == false && anomaliesCount > 0 && !restarted && levels[currentLevel].specialLevel)
        {
            StartCoroutine(LoadRevolutionCR());
            createTopicsScript.revolutionObject.SetActive(true);
            revolution = true;
        }
        else if(isRunning == false && anomaliesCount > 0 && !restarted)
        {
            restarted =  true;
            reasonTextUI.text = "Time ran out...";
            RestartDay(false);
        }

        if (isRunning)
        {
            UpdateTimer();
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentLevel = 0;
            StartLevel(0);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentLevel = 1;
            StartLevel(1);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentLevel = 2;
            StartLevel(2);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentLevel = 3;
            StartLevel(3);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentLevel = 4;
            StartLevel(4);
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentLevel = 5;
            StartLevel(5);
        }

        if (levels[currentLevel].specialLevel)
        {
            specialLevelGreyscale -= (1/levels[currentLevel].timer) * Time.deltaTime * 2;

            if (specialLevelGreyscale < 0)
            {
                specialLevelGreyscale = 0;
            }

            spritesGreyscaleMaterial.SetFloat("_GrayscaleAmount", specialLevelGreyscale);
            groundGreyscaleMaterial.SetFloat("_Greyscale", specialLevelGreyscale);
        }

    }

    private CivilianFaultType PriorityFaultType()
    {
        if (specialDayRevolutionApproaching)
        {
            //Ignore singing and radio to force more cloves after a certain time on the special level
            return faultTypes[UnityEngine.Random.Range(0, faultTypes.Count - 2)];
        }
        else if (newTopics > 0)
        {
            newTopics--;
            return CivilianFaultType.Talking;
        }
        else if (newBooks > 0)
        {
            newBooks--;
            return CivilianFaultType.Item;
        }
        else if (newHats > 0)
        {
            newHats--;
            return CivilianFaultType.Fashion;
        }
        else if (newSinging > 0)
        {
            newSinging--;
            return CivilianFaultType.Singing;
        }
        else if (newRadio > 0)
        {
            newRadio--;
            return CivilianFaultType.Radio;
        }
        else
        {
            return faultTypes[UnityEngine.Random.Range(0, faultTypes.Count)];
        }
    }


    private IEnumerator LoadRevolutionCR()
    {
        yield return new WaitForSeconds(3.5f);

        forceLoadScene.LoadRevolution();
    }

    private void EndDay()
    {
        playerMov.StartMoving(false);
        currentLevel++;
        reasonTextUI.text = "Stop any suspicious activity";
        createTopicsScript.goodJobObject.SetActive(true);
        StartCoroutine(EndLevelCoroutine());
    }

    public void RestartDay(bool died)
    {
        playerMov.StartMoving(false);
        if (died)
        {
            /*if(levels[currentLevel].specialLevel)
            {
                reasonTextUI.text = "";
                createTopicsScript.revolutionObject.SetActive(true);
                revolution = true;
            }
            else*/
            reasonTextUI.text = "Stop censoring the innocent!";
        }
        StartLevel(currentLevel);
    }

    void UpdateTimer()
    {

        if (isRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // Decrease time
        }
        else if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            isRunning = false;
        }

        pointerPivot.transform.localEulerAngles = new Vector3 (0f, 0f, Mathf.Lerp(0, 360, timeRemaining/ levels[currentLevel].timer));

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}"; // Format as MM:SS
    }

    public void StartLevel(int level)
    {

        /*foreach (GameObject go in tabUI)
        {
            go.SetActive(false);
        }*/
        StartCoroutine(StartLevelCR(level));
    }

    public IEnumerator StartLevelCR(int level)
    {

        fadeScreen.Fade(false);
        fadeScreen.SetDay((20 + level).ToString(), true);

        yield return new WaitForSeconds(1f);

        FaultManager.Instance.ResetFaults(levels[level].numberOfFaults);

        if (level == 0)
        {
            createTopicsScript.InitializeCensorshipItems();
        }

        civilianBrainScript.ClearCivillians();
        createTopicsScript.badgeCover1.SetActive(false);
        createTopicsScript.badgeCover2.SetActive(false);

        if (currentLevel > 0)
        {
            newTopics = levels[currentLevel].badTopicsNumber - levels[currentLevel - 1].badTopicsNumber;
            newBooks = levels[currentLevel].badBooksNumber - levels[currentLevel - 1].badBooksNumber;
            newHats = levels[currentLevel].badHatsNumber - levels[currentLevel - 1].badHatsNumber;

            if (levels[currentLevel].badSinging && !levels[currentLevel - 1].badSinging)
            {
                newSinging = 1;
            }

            if (levels[currentLevel].badRadio && !levels[currentLevel - 1].badRadio)
            {
                newRadio = 1;
            }
        }

        yield return new WaitForSeconds(2f);

        uiObject.SetActive(true);
        logosCanvas.SetActive(false);
        spritesGreyscaleMaterial.SetFloat("_GrayscaleAmount", levels[currentLevel].initialGreyscale);
        groundGreyscaleMaterial.SetFloat("_Greyscale", levels[currentLevel].initialGreyscale);
        if (levels[currentLevel].specialLevel)
        {
            specialLevelGreyscale = levels[currentLevel].initialGreyscale;
            musicianNPC.SetActive(true);
        }
        else
        {
            musicianNPC.SetActive(false);
        }
        fadeScreen.Fade(true);


        if (levels[level].specialLevel)
            createTopicsScript.ActivateSpecialDay();

        numberOfPeopleSpawned = 0;

        faultTypes.Clear();
        indexOfFaults.Clear();
        specialDayRevolutionApproaching = false;

        InitalizeAvailableSpots(levels[level].numberOfPeople);

        for (int i = 0; i < levels[level].numberOfFaults; i++)
        {
            indexOfFaults.Add(availableSlots[i]);
        }

        if (currentLevel == 0)
        {
            createTopicsScript.SetNumberOfFaultTypes(levels[level].badTopicsNumber,
                                                    newTopics,
                                                    levels[level].badHatsNumber,
                                                    newHats,
                                                    levels[level].badBooksNumber,
                                                    newBooks,
                                                    levels[level].badSinging,
                                                    newSinging,
                                                    levels[level].badRadio,
                                                    newRadio,
                                                    levels[level].badFootball,
                                                    true
            );
        }
        else
        {
            createTopicsScript.SetNumberOfFaultTypes(levels[level].badTopicsNumber,
                                                    newTopics,
                                                    levels[level].badHatsNumber,
                                                    newHats,
                                                    levels[level].badBooksNumber,
                                                    newBooks,
                                                    levels[level].badSinging,
                                                    newSinging,
                                                    levels[level].badRadio,
                                                    newRadio,
                                                    levels[level].badFootball,
                                                    false
            );
        }



        createTopicsScript.CreateNewTopics();
        createTopicsScript.CreateNewHats();
        createTopicsScript.CreateNewBooks();


        if (levels[level].badTopicsNumber > 0)
            faultTypes.Add(CivilianFaultType.Talking);

        if (levels[level].badHatsNumber > 0)
            faultTypes.Add(CivilianFaultType.Fashion);

        if (levels[level].badBooksNumber > 0)
            faultTypes.Add(CivilianFaultType.Item);

        if (levels[level].badSinging)
        {
            createTopicsScript.CreateSinging(true);
            faultTypes.Add(CivilianFaultType.Singing);
        }
        else
        {
            createTopicsScript.CreateSinging(false);
        }

        if (levels[level].badRadio)
        {
            createTopicsScript.CreateRadio(true);
            faultTypes.Add(CivilianFaultType.Radio);
        }
        else
        {
            createTopicsScript.CreateRadio(false);
        }

        createTopicsScript.UpdateCensorship();


        spawning = true;

        isRunning = true;
        timeRemaining = levels[level].timer;

        restarted = false;
        anomaliesCount = 0;
        playerMov.StartMoving(true);
    }

    private void InitalizeAvailableSpots(int amount)
    {

        availableSlots.Clear();

        for (int i = 0; i < amount; i++)
        {
            availableSlots.Add(i);
        }

        Shuffle(availableSlots);
    }

    public static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int swapIndex = random.Next(0, i + 1);
            (list[i], list[swapIndex]) = (list[swapIndex], list[i]);
        }
    }

    private IEnumerator EndLevelCoroutine()
    {
        yield return new WaitForSeconds(2f);
        StartLevel(currentLevel);
    }

    public void LoadAboutScene()
    {
        StartCoroutine(LoadAboutDelay());
    }

    private IEnumerator LoadAboutDelay()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2);
    }
}
