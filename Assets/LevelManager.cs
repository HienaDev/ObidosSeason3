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

        public int maximumAnomaliesActive;

        public bool specialLevel;
        public int initialChance;
        public int chanceIncrease;

        [Range(0, 1)]
        public float initialGreyscale;
    }

    [SerializeField] private FaultManager faultManager;

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
                int chanceOfspawn = UnityEngine.Random.Range(0, 100);

                if (chanceOfspawn < levels[currentLevel].initialChance)
                {
                    CivilianFaultType randomFault = faultTypes[UnityEngine.Random.Range(0, faultTypes.Count)];
                    civilianBrainScript.CreateNewCivilian(randomFault);
                    faultManager.AddFault();
                }
                else
                {
                    Debug.Log("Good Civillian Spawned");
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
                        Debug.Log("Bad Civillian Spawned");
                        CivilianFaultType randomFault = faultTypes[UnityEngine.Random.Range(0, faultTypes.Count)];
                        civilianBrainScript.CreateNewCivilian(randomFault);
                        faultManager.AddFault();
                    }
                    else
                    {
                        // Spawn Good Civillian
                        Debug.Log("Good Civillian Spawned");
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

        if (indexOfFaults.Count <= 0 && anomaliesCount <= 0 && isRunning)
        {
            isRunning = false;
            EndDay();
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


    private IEnumerator LoadRevolutionCR()
    {
        yield return new WaitForSeconds(2.5f);

        forceLoadScene.LoadRevolution();
    }

    private void EndDay()
    {
        currentLevel++;
        reasonTextUI.text = "Stop any suspicious activity";
        createTopicsScript.goodJobObject.SetActive(true);
        StartCoroutine(EndLevelCoroutine());
    }

    public void RestartDay(bool died)
    {
        if(died)
        {
            if(levels[currentLevel].specialLevel)
            {
                reasonTextUI.text = "";
                createTopicsScript.revolutionObject.SetActive(true);
                revolution = true;
            }
            else
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

        FaultManager.Instance.ResetFaults();

        civilianBrainScript.ClearCivillians();
        createTopicsScript.badgeCover1.SetActive(false);
        createTopicsScript.badgeCover2.SetActive(false);
        StartCoroutine(StartLevelCR(level));
    }

    public IEnumerator StartLevelCR(int level)
    {

        fadeScreen.Fade(false);
        fadeScreen.SetDay((21 + level).ToString(), true);

        yield return new WaitForSeconds(3f);
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

        InitalizeAvailableSpots(levels[level].numberOfPeople);

        for (int i = 0; i < levels[level].numberOfFaults; i++)
        {
            indexOfFaults.Add(availableSlots[i]);
        }


        createTopicsScript.SetNumberOfFaultTypes(levels[level].badTopicsNumber,
                                                 levels[level].badHatsNumber,
                                                 levels[level].badBooksNumber);

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


        spawning = true;

        isRunning = true;
        timeRemaining = levels[level].timer;

        restarted = false;
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
}
