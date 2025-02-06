using System.Collections;
using UnityEngine;

public class TalkingBubble : MonoBehaviour
{

    private CreateTopics topicManager;
    private Sprite topicSprite;
    private Transform player;
    [SerializeField] private GameObject bubbleParent;
    [SerializeField] private float distanceToSeeConversation = 5f;
    [SerializeField] private GameObject cantSeeSprite;
    [SerializeField] private SpriteRenderer symbolPlace;

    [SerializeField] private SpriteRenderer hatSprite;
    private Sprite hat;

    private bool talking = false;
    //private bool topicIsFarAway = true;
    public bool badTopic = false;

    public bool badHat = false;


    [SerializeField] private GameObject book;
    [SerializeField] private SpriteRenderer bookCover;
    [SerializeField] private SpriteRenderer bookSymbol;
    private (Sprite, Color) bookObject;
    public bool badBook = false;
    private CivilianFaultType _civilianFaultType;
    public void Initialize(CivilianFaultType type)
    {
        _civilianFaultType = type;
        topicManager = FindAnyObjectByType<CreateTopics>();
        player = FindAnyObjectByType<PlayerMovement>().gameObject.transform;
        GetRandomHat(type == CivilianFaultType.Fashion);
        GetRandomBook(type == CivilianFaultType.Item);
    }

    private void FixedUpdate()
    {
        if (talking)
            if (Vector2.Distance(player.position, transform.position) > distanceToSeeConversation)// && !topicIsFarAway)
            {
                if (cantSeeSprite != null)
                {
                    symbolPlace.gameObject.SetActive(false);
                    cantSeeSprite.SetActive(true);
                }
                //topicIsFarAway = true;
            }
            else if (Vector2.Distance(player.position, transform.position) < distanceToSeeConversation)// && topicIsFarAway)
            {
                if (topicSprite != null)
                {
                    symbolPlace.gameObject.SetActive(true);
                    cantSeeSprite.SetActive(false);
                }
                //topicIsFarAway = false;
            }
    }

    public void GetRandomHat(bool badHatToggle)
    {
        (hat, badHat) = topicManager.GetRandomHat(badHatToggle);

        hatSprite.sprite = hat;
    }

    public void GetRandomBook(bool badBookToggle)
    {
        (bookObject, badBook) = topicManager.GetRandomBook(badBookToggle);

        if (bookObject.Item1 == null)
            return;

        book.SetActive(true);

        bookCover.color = bookObject.Item2;
        bookSymbol.color = bookObject.Item2;
        bookSymbol.sprite = bookObject.Item1;
    }

    public void StartTalking()
    {
        talking = true;
        bubbleParent.SetActive(true);
        (topicSprite, badTopic) = topicManager.GetRandomTopic(_civilianFaultType == CivilianFaultType.Talking);
        Debug.Log("talking: " + topicSprite.name);
        symbolPlace.sprite = topicSprite;
    }

    public void StopTalking()
    {
        talking = false;
        bubbleParent.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, distanceToSeeConversation);
    }
}
