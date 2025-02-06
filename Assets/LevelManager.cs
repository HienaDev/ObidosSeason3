using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using TMPro;

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
    private bool isRunning = false;
    private float timeRemaining = 0f;

    [SerializeField] private CivilianBrain civilianBrainScript;

    private List<CivilianFaultType> faultTypes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        availableSlots = new List<int>();
        indexOfFaults = new List<int>();
        faultTypes = new List<CivilianFaultType> ();

    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.P))
        {
            StartLevel(1);
        }

        if(spawning && Time.time - justSpawned > levels[currentLevel].spawnRate)
        {

            if (levels[currentLevel].specialLevel)
            {
                int chanceOfspawn = UnityEngine.Random.Range(0, 100);

                if(chanceOfspawn < levels[currentLevel].initialChance)
                {
                    CivilianFaultType randomFault = faultTypes[UnityEngine.Random.Range(0, faultTypes.Count)];
                    civilianBrainScript.CreateNewCivilian(randomFault);
                    faultManager.AddFault();
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

        if(isRunning)
        {
            UpdateTimer();
        }
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

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}"; // Format as MM:SS
    }

    public void StartLevel(int level)
    {

        if (levels[currentLevel].specialLevel)
            createTopicsScript.ActivateSpecialDay();

        // turn level from number to index
        currentLevel = level - 1;
        numberOfPeopleSpawned = 0;

        faultTypes.Clear();
        indexOfFaults.Clear();

        InitalizeAvailableSpots(levels[currentLevel].numberOfPeople);

        for (int i = 0; i < levels[currentLevel].numberOfFaults; i++)
        {
            indexOfFaults.Add(availableSlots[i]);
        }


        createTopicsScript.SetNumberOfFaultTypes(levels[currentLevel].badTopicsNumber,
                                                 levels[currentLevel].badHatsNumber,
                                                 levels[currentLevel].badBooksNumber);

        createTopicsScript.CreateNewTopics();
        createTopicsScript.CreateNewHats();
        createTopicsScript.CreateNewBooks();


        if (levels[currentLevel].badTopicsNumber > 0)
            faultTypes.Add(CivilianFaultType.Talking);

        if (levels[currentLevel].badHatsNumber > 0)
            faultTypes.Add(CivilianFaultType.Fashion);

        if (levels[currentLevel].badBooksNumber > 0)
            faultTypes.Add(CivilianFaultType.Item);


        spawning = true;

        isRunning = true;
        timeRemaining = levels[currentLevel].timer;
    }

    private void InitalizeAvailableSpots(int amount)
    {

        availableSlots.Clear();

        for (int i = 0;i < amount;i++)
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
