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
    private List<TalkingData.TalkingTopics> tempGoodTopics;
    public List<TalkingData.TalkingTopics> GoodTopics => goodTopics;
    private List<TalkingData.TalkingTopics> badTopics;
    public List<TalkingData.TalkingTopics> BadTopics => badTopics;
    [SerializeField] private int numberOfBadTopics = 2;
    [Header("HATS")]
    private List<TalkingData.TalkingTopics> goodHats;
    private List<TalkingData.TalkingTopics> tempGoodHats;
    public List<TalkingData.TalkingTopics> GoodHats => goodHats;
    private List<TalkingData.TalkingTopics> badHats;
    public List<TalkingData.TalkingTopics> BadHats => badHats;
    [SerializeField] private int numberOfBadHats = 2;

    [SerializeField, Range(0, 100), Header("BOOKS")] private int chanceOfBook = 30;
    [SerializeField, Range(1, 3)] private int numberOfColors = 3;
    private List<(Sprite, Color)> books;
    private List<(Sprite, Color)> tempBooks;
    public List<(Sprite, Color)> Books => books;
    private List<(Sprite, Color)> badBooks;
    public List<(Sprite, Color)> BadBooks => badBooks;
    [SerializeField] private int numberOfBadBooks = 2;

    [Header("UI")]
    [SerializeField] private Image[] modelsForHats;
    [SerializeField] private Sprite[] spritesModelsForHats;
    [SerializeField] private Image forbiddenSingingImage;
    [SerializeField] private Image forbiddenRadioImage;
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

    public GameObject badgeCover1;
    public GameObject badgeCover2;


    public GameObject goodJobObject;
    public GameObject revolutionObject;

    public bool specialDay = false;

    private bool badSinging = false;
    private bool badRadio = false;
    private bool badFootball = false;

    private int numberOfCensoredThings = 0;
    [SerializeField]
    private Image[] allCensorImages;
    private List<Sprite> censoredSprites;

    public void ActivateSpecialDay()
    {
        specialDay = true;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //CreateNewTopics();
        //CreateNewHats();
        //CreateNewBooks();
    }

    public void SetNumberOfFaultTypes(int amountBadTopics, int amountBadHats, int amountOfBadBooks, bool singing, bool radio, bool football)
    {
        numberOfCensoredThings = 0;
        numberOfBadTopics = amountBadTopics;
        numberOfBadHats = amountBadHats;
        numberOfBadBooks = amountOfBadBooks;
        numberOfCensoredThings += amountBadTopics + amountBadHats + amountOfBadBooks;
        if (singing) { numberOfCensoredThings++; };
        if (radio) { numberOfCensoredThings++; };
        if (football) {
            numberOfCensoredThings++;
            badFootball = football;
        }
        ;
        censoredSprites = new List<Sprite>();
    }

    public void CreateNewBooks()
    {

        currentForbiddenBook = 0;

        books = new List<(Sprite, Color)>();
        tempBooks = new List<(Sprite, Color)>();
        badBooks = new List<(Sprite, Color)>();

        foreach (Sprite shape in data.bookShapes)
        {
            for (int i = 0; i < numberOfColors; i++)
            {
                books.Add((shape, data.bookColors[i]));
                tempBooks.Add((shape, data.bookColors[i]));
            }
        }

        for (int i = 0; i < numberOfBadBooks; i++)
        {
            //int randomBadBook = Random.Range(0, books.Count);
            //(Sprite, Color) badBook = books[randomBadBook];
            (Sprite, Color) badBook = tempBooks[i];
            books.Remove(badBook);
            badBooks.Add(badBook);
            //forbiddenBookImages[currentForbiddenBook].gameObject.SetActive(true);
            //forbiddenBookImages[currentForbiddenBook].color = badBook.Item2;
            //forbiddenBookShapes[currentForbiddenBook].color = badBook.Item2;
            //forbiddenBookShapes[currentForbiddenBook].sprite = badBook.Item1;
            censoredSprites.Add(badBook.Item1);
            currentForbiddenBook++;
        }

        if (specialDay)
        {
            Debug.Log("Special day");
            (Sprite, Color) badBook = (data.specialItem, Color.white);
            //forbiddenBookImages[currentForbiddenBook].gameObject.SetActive(true);
            //forbiddenBookImages[currentForbiddenBook].sprite = badBook.Item1;
            //forbiddenBookCovers[currentForbiddenBook].gameObject.SetActive(false);
            //forbiddenBookShapes[currentForbiddenBook].gameObject.SetActive(false);
            censoredSprites.Add(badBook.Item1);
            currentForbiddenBook++;
        }

        for (int i = currentForbiddenBook; i < forbiddenBookImages.Length; i++)
        {
            forbiddenBookShapes[i].gameObject.SetActive(false);
        }

        Debug.Log("currentForbiddenBook" + currentForbiddenBook);

        currentForbiddenBook = 0;
    }       

    public ((Sprite, Color), bool) GetRandomBook(bool badBookToggle, bool special = false)
    {

        if (special && badBookToggle)
        {
            return ((data.specialItem, Color.white), true);
        }

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

        currentForbiddenHat = 0;

        goodHats = new List<TalkingData.TalkingTopics>();
        tempGoodHats = new List<TalkingData.TalkingTopics>();
        badHats = new List<TalkingData.TalkingTopics>();



        goodHats = data.hats.OfType<TalkingData.TalkingTopics>().ToList();
        tempGoodHats = data.hats.OfType<TalkingData.TalkingTopics>().ToList();

        for (int i = 0; i < numberOfBadHats; i++)
        {
            //int randomBadHat = Random.Range(0, goodHats.Count);
            //TalkingData.TalkingTopics badHat = goodHats[randomBadHat];
            TalkingData.TalkingTopics badHat = tempGoodHats[i];
            goodHats.Remove(badHat);
            badHats.Add(badHat);
            forbiddenHatsImages[currentForbiddenHat].sprite = badHat.symbol;
            forbiddenHatsImages[currentForbiddenHat].color = Color.white;
            //modelsForHats[currentForbiddenHat].sprite = spritesModelsForHats[Random.Range(0, spritesModelsForHats.Length)];
            forbiddenHatsImages[currentForbiddenHat].gameObject.SetActive(true);
            //modelsForHats[currentForbiddenHat].gameObject.SetActive(true);
            censoredSprites.Add(badHat.symbol);
            currentForbiddenHat++;
        }

        if (specialDay)
        {
            
            forbiddenHatsImages[currentForbiddenHat].sprite = data.specialHat;
            forbiddenHatsImages[currentForbiddenHat].color = Color.white;
            forbiddenHatsImages[currentForbiddenHat].gameObject.SetActive(true);
            //modelsForHats[currentForbiddenHat].gameObject.SetActive(true);
            censoredSprites.Add(data.specialHat);
            currentForbiddenHat++;
        }

        for (int i = currentForbiddenHat; i < forbiddenHatsImages.Length; i++)
        {
            forbiddenHatsImages[i].gameObject.SetActive(false);
            //modelsForHats[i].gameObject.SetActive(false);
        }

        currentForbiddenHat = 0;
    }

    public (Sprite, bool) GetRandomHat(bool badHatToggle, bool special = false)
    {

        if(special && badHatToggle)
        {
            return (data.specialHat, true);
        }

        if (badHatToggle)
        {
            int randomHat = Random.Range(0, badHats.Count);
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
        currentForbiddenWord = 0;
        goodTopics = new List<TalkingData.TalkingTopics>();
        tempGoodTopics = new List<TalkingData.TalkingTopics>();
        badTopics = new List<TalkingData.TalkingTopics>();

        winningFootBallTeam = Random.Range(0, data.footballTeamWinning.Length);

        //winningTeam.sprite = data.footballTeamWinning[winningFootBallTeam];

        if (badFootball)
        {
            censoredSprites.Add(data.footballTeamWinning[winningFootBallTeam]);
        }

        goodTopics = data.topics.OfType<TalkingData.TalkingTopics>().ToList();
        tempGoodTopics = data.topics.OfType<TalkingData.TalkingTopics>().ToList();

        for (int i = 0; i < numberOfBadTopics; i++)
        {
            //int randomBadTopic = Random.Range(0, goodTopics.Count);
            //TalkingData.TalkingTopics badTopic = goodTopics[randomBadTopic];
            TalkingData.TalkingTopics badTopic = tempGoodTopics[i];
            goodTopics.Remove(badTopic);
            badTopics.Add(badTopic);
            forbiddenWordsImages[currentForbiddenWord].sprite = badTopic.symbol;
            forbiddenWordsImages[currentForbiddenWord].color = Color.white;
            forbiddenWordsImages[currentForbiddenWord].gameObject.SetActive(true);
            censoredSprites.Add(badTopic.symbol);
            currentForbiddenWord++;
        }

        if (specialDay)
        {

            forbiddenWordsImages[currentForbiddenWord].sprite = data.specialTopic;
            forbiddenWordsImages[currentForbiddenWord].color = Color.white;
            forbiddenWordsImages[currentForbiddenWord].gameObject.SetActive(true);
            censoredSprites.Add(data.specialTopic);
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

    public (Sprite, bool) GetRandomTopic(bool badTopicToggle, bool special = false)
    {

        if(special && badTopicToggle)
        {
            return (data.specialTopic, true);
        }

        if (badTopicToggle)
        {
            int footballRNG = Random.Range(0, 100);

            if (footballRNG < chanceOfFootballTopic && badFootball)
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

            if (footballRNG < chanceOfFootballTopic && badFootball)
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

    public void CreateSinging(bool badSinging)
    {
        //forbiddenSingingImage.gameObject.SetActive(badSinging);
        //forbiddenSingingImage.sprite = data.singing;
        this.badSinging = badSinging;

        if(badSinging )
        {
            censoredSprites.Add(data.singing);
        }
    }

    public void CreateRadio(bool badRadio)
    {
        //forbiddenRadioImage.gameObject.SetActive(badRadio);
        //forbiddenRadioImage.sprite = data.radio;
        this.badRadio = badRadio;

        if (badRadio)
        {
            censoredSprites.Add(data.radio);
        }
    }

    /// <summary>
    /// This method returns true to allow for NPCS to sing only if its not a day with censurable singing.
    /// </summary>
    /// <returns></returns>
    public bool GetRandomSinging()
    {
        if (!badSinging)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// This method returns true to allow for NPCS to have radios only if its not a day with censurable radio.
    /// </summary>
    /// <returns></returns>
    public bool GetRandomRadio()
    {
        if (!badRadio)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateCensorship()
    {
        for (int i = 0; i < allCensorImages.Length; i++)
        {
            if (i < censoredSprites.Count)
            {
                allCensorImages[i].gameObject.SetActive(true);
                allCensorImages[i].sprite = censoredSprites[i];
            }
            else
            {
                allCensorImages[i].gameObject.SetActive(false);
            }
        }
    }
}
