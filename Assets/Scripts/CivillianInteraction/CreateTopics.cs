using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;


public class CreateTopics : MonoBehaviour
{

    [SerializeField, Header("BOOKS")] private TalkingData data;
    [SerializeField, Range(0, 100), Header("TOPICS")] private int chanceOfFootballTopic = 30;
    private int winningFootBallTeam = 0;
    private int losingFootBallTeam = 1;
    private List<TalkingData.TalkingTopics> goodTopics;
    public List<TalkingData.TalkingTopics> GoodTopics => goodTopics;
    private List<TalkingData.TalkingTopics> badTopics;
    public List<TalkingData.TalkingTopics> BadTopics => badTopics;
    [SerializeField] private int numberOfBadTopics = 2;
    [Header("HATS")]
    private List<TalkingData.TalkingTopics> goodHats;
    public List<TalkingData.TalkingTopics> GoodHats => goodHats;
    private List<TalkingData.TalkingTopics> badHats;
    public List<TalkingData.TalkingTopics> BadHats => badHats;
    [SerializeField] private int numberOfBadHats = 2;

    [SerializeField, Range(0, 100), Header("BOOKS")] private int chanceOfBook = 30;
    [SerializeField, Range(1, 3)] private int numberOfColors = 3;
    private List<(Sprite, Color)> books;
    public List<(Sprite, Color)> Books => books;
    private List<(Sprite, Color)> badBooks;
    public List<(Sprite, Color)> BadBooks => badBooks;
    [SerializeField] private int numberOfBadBooks = 2;

    [Header("UI")]
    [SerializeField] private Image[] forbiddenWordsImages;
    private int currentForbiddenWord = 0;

    [SerializeField] private Image[] forbiddenHatsImages;
    private int currentForbiddenHat = 0;

    [SerializeField] private Image[] forbiddenBookImages;
    [SerializeField] private Image[] forbiddenBookCovers;
    [SerializeField] private Image[] forbiddenBookShapes;
    private int currentForbiddenBook = 0;

    [SerializeField] private Image winningTeam;
    [SerializeField] private Image losingTeam;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //CreateNewTopics();
        //CreateNewHats();
        //CreateNewBooks();
    }

    public void SetNumberOfFaultTypes(int amountBadTopics,  int amountBadHats, int amountOfBadBooks)
    {
        numberOfBadTopics = amountBadTopics;
        numberOfBadHats = amountBadHats;
        numberOfBadBooks = amountOfBadBooks;
    }

    public void CreateNewBooks()
    {
        books = new List<(Sprite, Color)>();
        badBooks = new List<(Sprite, Color)>();

        foreach (Sprite shape in data.bookShapes)
        {
            for (int i = 0; i < numberOfColors; i++)
            {
                books.Add((shape, data.bookColors[i]));
            }
        }

        for (int i = 0; i < numberOfBadBooks; i++)
        {
            int randomBadBook = Random.Range(0, books.Count);
            (Sprite, Color) badBook = books[randomBadBook];
            books.Remove(badBook);
            badBooks.Add(badBook);
            forbiddenBookImages[currentForbiddenBook].gameObject.SetActive(true);
            forbiddenBookCovers[currentForbiddenBook].color = badBook.Item2;
            forbiddenBookShapes[currentForbiddenBook].color = badBook.Item2;
            forbiddenBookShapes[currentForbiddenBook].sprite = badBook.Item1;
            currentForbiddenBook++;
        }

        for (int i = currentForbiddenBook; i < forbiddenBookImages.Length; i++)
        {
            forbiddenBookImages[i].gameObject.SetActive(false);
        }

        Debug.Log("currentForbiddenBook" + currentForbiddenBook);

        currentForbiddenBook = 0;
    }

    public ((Sprite, Color), bool) GetRandomBook(bool badBookToggle)
    {

        if (!badBookToggle)
        {
            int bookRNG = Random.Range(0, 100);

            if (bookRNG > chanceOfBook)
                return ((null, Color.white), false);

            int randomBook = Random.Range(0, books.Count);

            (Sprite, Color) goodBook = books[randomBook];
            return (goodBook, false);


        }
        else
        {

            int randomBook = Random.Range(0, badBooks.Count);
            (Sprite, Color) badBook = badBooks[randomBook];
            return (badBook, true);


        }

    }

    public void CreateNewHats()
    {
        goodHats = new List<TalkingData.TalkingTopics>();
        badHats = new List<TalkingData.TalkingTopics>();



        goodHats = data.hats.OfType<TalkingData.TalkingTopics>().ToList();

        for (int i = 0; i < numberOfBadHats; i++)
        {
            int randomBadHat = Random.Range(0, goodHats.Count);
            TalkingData.TalkingTopics badHat = goodHats[randomBadHat];
            goodHats.Remove(badHat);
            badHats.Add(badHat);
            forbiddenHatsImages[currentForbiddenHat].sprite = badHat.symbol;
            forbiddenHatsImages[currentForbiddenHat].color = Color.white;
            currentForbiddenHat++;
        }

        for (int i = currentForbiddenHat; i < forbiddenHatsImages.Length; i++)
        {
            forbiddenHatsImages[i].gameObject.SetActive(false);
        }

        currentForbiddenHat = 0;
    }

    public (Sprite, bool) GetRandomHat(bool badHatToggle)
    {

        if (badHatToggle)
        {
            int randomHat = Random.Range(0, badBooks.Count);
            // Bad Hat
            TalkingData.TalkingTopics badHat = badHats[randomHat];
            return (badHat.symbol, true);
        }
        else
        {

            int randomHat = Random.Range(0, goodHats.Count);
            // Good Hat
            TalkingData.TalkingTopics goodHat = goodHats[randomHat];
            return (goodHat.symbol, false);
        }

    }

    public void CreateNewTopics()
    {
        goodTopics = new List<TalkingData.TalkingTopics>();
        badTopics = new List<TalkingData.TalkingTopics>();

        winningFootBallTeam = Random.Range(0, data.footballTeamWinning.Length);

        winningTeam.sprite = data.footballTeamWinning[winningFootBallTeam];

        do
        {
            losingFootBallTeam = Random.Range(0, data.footballTeamWinning.Length);
        } while (winningFootBallTeam == losingFootBallTeam);

        losingTeam.sprite = data.footballTeamLogo[losingFootBallTeam];

        goodTopics = data.topics.OfType<TalkingData.TalkingTopics>().ToList();

        for (int i = 0; i < numberOfBadTopics; i++)
        {
            int randomBadTopic = Random.Range(0, goodTopics.Count);
            TalkingData.TalkingTopics badTopic = goodTopics[randomBadTopic];
            goodTopics.Remove(badTopic);
            badTopics.Add(badTopic);
            forbiddenWordsImages[currentForbiddenWord].sprite = badTopic.symbol;
            forbiddenWordsImages[currentForbiddenWord].color = Color.white;
            currentForbiddenWord++;
        }

        List<TalkingData.TalkingTopics> tempList = data.alwaysBadTopics.OfType<TalkingData.TalkingTopics>().ToList();

        badTopics.Concat(tempList);

        for (int i = currentForbiddenWord; i < forbiddenWordsImages.Length; i++)
        {
            forbiddenWordsImages[i].gameObject.SetActive(false);
        }

        currentForbiddenWord = 0;
    }

    public (Sprite, bool) GetRandomTopic(bool badTopicToggle)
    {

        if (badTopicToggle)
        {
            int footballRNG = Random.Range(0, 100);

            if(footballRNG < chanceOfFootballTopic)
            {
                int randomFootBallTeam = 0;
                do
                {
                    randomFootBallTeam = Random.Range(0, data.footballTeamWinning.Length);
                } while (winningFootBallTeam == randomFootBallTeam);

                return (data.footballTeamWinning[randomFootBallTeam], true);
            }
            else
            {
                int randomTopic = Random.Range(0, badTopics.Count);
                // Bad topic
                TalkingData.TalkingTopics badTopic = badTopics[randomTopic];
                return (badTopic.symbol, true);
            }
        }
        else
        {
            int footballRNG = Random.Range(0, 100);

            if (footballRNG < chanceOfFootballTopic)
            {
                return (data.footballTeamWinning[winningFootBallTeam], false);
            }
            else
            {
                int randomTopic = Random.Range(0, goodTopics.Count);
                // Good Topic
                TalkingData.TalkingTopics goodTopic = goodTopics[randomTopic];
                return (goodTopic.symbol, false);
            }

        }

    }
}
