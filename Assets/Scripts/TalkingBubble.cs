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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        topicManager = FindAnyObjectByType<CreateTopics>();
        player = FindAnyObjectByType<PlayerMovement>().gameObject.transform;

        yield return new WaitForSeconds(0.1f);
        GetRandomHat();
        GetRandomBook();
        //StartTalking();
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

    public void GetRandomHat()
    {
        (hat, badHat) = topicManager.GetRandomHat();

        hatSprite.sprite = hat;
    }

    public void GetRandomBook()
    {
        (bookObject, badBook) = topicManager.GetRandomBook();

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
        (topicSprite, badTopic) = topicManager.GetRandomTopic();
        Debug.Log("talking: " + topicSprite.name);
        symbolPlace.sprite = topicSprite;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, distanceToSeeConversation);
    }
}
