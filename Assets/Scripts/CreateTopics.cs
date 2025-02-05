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

    private List<TalkingData.TalkingTopics> goodHats;
    public List<TalkingData.TalkingTopics> GoodHats => goodHats;
    private List<TalkingData.TalkingTopics> badHats;
    public List<TalkingData.TalkingTopics> BadHats => badHats;
    [SerializeField] private int numberOfBadHats = 2;

    [SerializeField, Range(0, 100), Header("BOOKS")] private int chanceOfBook = 30;
    [SerializeField, Range(1, 3)] private int numberOfColors = 3;
    private int currentBookColor = 0;
    private List<(Sprite, Color)> books;
    public List<(Sprite, Color)> Books => books;
    private List<(Sprite, Color)> badBooks;
    public List<(Sprite, Color)> BadBooks => badBooks;
    [SerializeField] private int numberOfBadBooks = 2;

    [SerializeField] private Image[] forbiddenWordsImages;
    private int currentForbiddenWord = 0;

    [SerializeField] private Image[] forbiddenHatsImages;
    private int currentForbiddenHat = 0;

    [SerializeField] private Image[] forbiddenBookImages;
    [SerializeField] private Image[] forbiddenBookCovers;
    [SerializeField] private Image[] forbiddenBookShapes;
    private int currentForbiddenBook = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateNewTopics();
        CreateNewHats();
        CreateNewBooks();
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
            forbiddenBookImages[currentForbiddenBook].gameObject.SetActive(false);
        }

        currentForbiddenBook = 0;
    }

    public ((Sprite, Color), bool) GetRandomBook()
    {
        int lengthOfBooks = books.Count + badBooks.Count;

        int randomBook = Random.Range(1, lengthOfBooks + 1);


        int bookRNG = Random.Range(0, 100);

        if (bookRNG > chanceOfBook)
            return ((null, Color.white), false);

        if (randomBook > books.Count)
        {
            // Bad Book
            (Sprite, Color) badBook = badBooks[randomBook - books.Count - 1];
            return (badBook, true);
        }
        else
        {
            // Good Book
            (Sprite, Color) goodBook = books[randomBook - 1];
            return (goodBook, false);
        }
    }

    public void CreateNewHats()
    {
        goodHats= new List<TalkingData.TalkingTopics>();
        badHats= new List<TalkingData.TalkingTopics>();



        goodHats= data.hats.OfType<TalkingData.TalkingTopics>().ToList();

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
            forbiddenHatsImages[currentForbiddenHat].gameObject.SetActive(false);
        }

        currentForbiddenHat = 0;
    }

    public (Sprite, bool) GetRandomHat()
    {
        int lengthOfHats = goodHats.Count + badHats.Count;

        int randomHat = Random.Range(1, lengthOfHats + 1);

        if (randomHat > goodHats.Count)
        {
            // Bad Hat
            TalkingData.TalkingTopics badHat = badHats[randomHat - goodHats.Count - 1];
            return (badHat.symbol, true);
        }
        else
        {
            // Good Hat
            TalkingData.TalkingTopics goodHat = goodHats[randomHat - 1];
            return (goodHat.symbol, false);
        }

    }

    public void CreateNewTopics()
    {
        goodTopics = new List<TalkingData.TalkingTopics>();
        badTopics = new List<TalkingData.TalkingTopics>();



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

        List< TalkingData.TalkingTopics> tempList = data.alwaysBadTopics.OfType<TalkingData.TalkingTopics>().ToList();

        badTopics.Concat(tempList);

        for (int i = currentForbiddenWord; i < forbiddenWordsImages.Length; i++)
        {
            forbiddenWordsImages[currentForbiddenWord].gameObject.SetActive(false);
        }

        currentForbiddenWord = 0;
    }

    public (Sprite, bool) GetRandomTopic()
    {
        int lengthOfTopics = goodTopics.Count + badTopics.Count;

        int randomTopic = Random.Range(1, lengthOfTopics + 1);

        if(randomTopic > goodTopics.Count)
        {
            // Bad topic
            TalkingData.TalkingTopics badTopic = badTopics[randomTopic - goodTopics.Count - 1];    
            return (badTopic.symbol, true);
        }
        else
        {
            // Good Topic
            TalkingData.TalkingTopics goodTopic = goodTopics[randomTopic - 1];
            return (goodTopic.symbol, false);
        }

    }
}
