using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using TMPro;
using System.Collections;
using static LevelManager;

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

        public int maximumAnomaliesActive;

        public bool specialLevel;
        public int initialChance;
        public int chanceIncrease;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        availableSlots = new List<int>();
        indexOfFaults = new List<int>();
        faultTypes = new List<CivilianFaultType>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartLevel(1);
        }

        if (spawning && Time.time - justSpawned > levels[currentLevel].spawnRate)
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
                createTopicsScript.revolutionObject.SetActive(true);
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
    }

    private void EndDay()
    {
        currentLevel++;
        reasonTextUI.text = "Stop any suspicious activity";
        createTopicsScript.goodJobObject.SetActive(true);
        StartLevel(currentLevel);
    }

    public void RestartDay(bool died)
    {
        if(died)
        {
            if(currentLevel == 4)
            {
                reasonTextUI.text = "";
                createTopicsScript.revolutionObject.SetActive(true);
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


        spawning = true;

        isRunning = true;
        timeRemaining = levels[level].timer;
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
}
