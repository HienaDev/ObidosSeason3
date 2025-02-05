using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;


public class CreateTopics : MonoBehaviour
{

    [SerializeField] private TalkingData data;
    private List<TalkingData.TalkingTopics> goodTopics;
    public List<TalkingData.TalkingTopics> GoodTopics => goodTopics;
    private List<TalkingData.TalkingTopics> badTopics;
    public List<TalkingData.TalkingTopics> BadTopics => badTopics;
    [SerializeField] private int numberOfBadTopics = 2;

    [SerializeField] private Image[] forbiddenWordsImages;
    private int currentForbiddenWord = 0;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateNewTopics();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateNewTopics()
    {
        goodTopics = new List<TalkingData.TalkingTopics>();
        badTopics = new List<TalkingData.TalkingTopics>();



        goodTopics = data.topics.OfType<TalkingData.TalkingTopics>().ToList();

        Debug.Log("Good Topic Before");
        foreach(TalkingData.TalkingTopics topic in goodTopics)
        {
            Debug.Log(topic.symbol.name);
        }

        Debug.Log("Bad Topic Before");
        foreach (TalkingData.TalkingTopics topic in badTopics)
        {
            Debug.Log(topic.symbol.name);
        }

        for (int i = 0; i < numberOfBadTopics; i++)
        {
            int randomBadTopic = Random.Range(0, goodTopics.Count);
            TalkingData.TalkingTopics badTopic = goodTopics[randomBadTopic];
            goodTopics.Remove(badTopic);
            badTopics.Add(badTopic);
            forbiddenWordsImages[currentForbiddenWord].sprite = badTopic.symbol;
            currentForbiddenWord++;
        }

        List< TalkingData.TalkingTopics> tempList = data.alwaysBadTopics.OfType<TalkingData.TalkingTopics>().ToList();

        badTopics.Concat(tempList);

        Debug.Log("Good Topic After");
        foreach (TalkingData.TalkingTopics topic in goodTopics)
        {
            Debug.Log(topic.symbol.name);
        }

        Debug.Log("Bad Topic After");
        foreach (TalkingData.TalkingTopics topic in badTopics)
        {
            Debug.Log(topic.symbol.name);
        }
    }

    public (Sprite, bool) GetRandomTopic()
    {
        int lengthOfTopics = goodTopics.Count + badTopics.Count;

        int randomTopic = Random.Range(1, lengthOfTopics + 1);

        if(randomTopic > goodTopics.Count)
        {
            // Bad topic
            TalkingData.TalkingTopics badTopic = badTopics[randomTopic - goodTopics.Count - 1];    
            return (badTopic.symbol, false);
        }
        else
        {
            // Good Topic
            TalkingData.TalkingTopics goodTopic = goodTopics[randomTopic - 1];
            return (goodTopic.symbol, false);
        }

    }
}
