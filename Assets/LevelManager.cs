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
    }


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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        availableSlots = new List<int>();
        indexOfFaults = new List<int>();


    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.P))
        {
            StartLevel(2);
        }

        if(spawning && Time.time - justSpawned > levels[currentLevel].spawnRate)
        {
            if (numberOfPeopleSpawned >= levels[currentLevel].numberOfPeople)
            {
                spawning = false;
            }
            else
            {
                if(indexOfFaults.Contains(numberOfPeopleSpawned))
                {
                    // Spawn Bad Civillian
                    Debug.Log("Bad Civillian Spawned");
                }
                else
                {
                    // Spawn Good Civillian
                    Debug.Log("Good Civillian Spawned");
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
        // turn level from number to index
        currentLevel = level - 1;
        numberOfPeopleSpawned = 0;
        
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
