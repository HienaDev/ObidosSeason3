using System.Collections;
using System.Runtime.CompilerServices;
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
    public CivilianFaultType _civilianFaultType;

    [field: Header("Talking animations")]
    [SerializeField] private float talkingPopUpAnimation = 0.3f;
    [SerializeField] private float timeSpentTalking = 4f;
    [SerializeField] private float timeTalkingRandomOffset = 1f;
    private float currentTimeToTalk = 0f;
    [SerializeField] private float talkingCooldown = 1f;
    private float currentTalkingCooldown = 0f;

    private Vector3 initialbubbleScale = Vector3.one;

    private Coroutine bubblePopCoroutine = null;

    public void Initialize(CivilianFaultType type)
    {
        _civilianFaultType = type;
        topicManager = FindAnyObjectByType<CreateTopics>();
        player = FindAnyObjectByType<PlayerMovement>().gameObject.transform;
        GetRandomHat(type == CivilianFaultType.Fashion);
        GetRandomBook(type == CivilianFaultType.Item);
        GetRandomTopic(type == CivilianFaultType.Talking);

        InitalizerSpeakingBubble();
    }

    private void InitalizerSpeakingBubble()
    {
        initialbubbleScale = bubbleParent.transform.localScale;
        bubbleParent.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) { }
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


    public void GetRandomTopic(bool badTopicToggle)
    {
        (topicSprite, badTopic) = topicManager.GetRandomTopic(badTopicToggle);
        Debug.Log("talking: " + topicSprite.name);
        symbolPlace.sprite = topicSprite;
    }

    public void StartTalking()
    {
        talking = true;
        bubbleParent.SetActive(true);

        if (bubblePopCoroutine != null)
            StopCoroutine(bubblePopCoroutine);

        bubblePopCoroutine = StartCoroutine(TalkingAnimation());
    }

    private IEnumerator TalkingAnimation()
    {
        currentTimeToTalk = timeSpentTalking + Random.Range(-timeTalkingRandomOffset, timeTalkingRandomOffset);

        float lerpValue = 0;
        

        while (lerpValue < 1)
        {
            lerpValue += Time.deltaTime / talkingPopUpAnimation;
            bubbleParent.transform.localScale = Vector3.Lerp(Vector3.zero, initialbubbleScale, lerpValue);
            yield return null;
        }

        yield return new WaitForSeconds(currentTimeToTalk);

        lerpValue = 0;

        while (lerpValue < 1)
        {
            lerpValue += Time.deltaTime / talkingPopUpAnimation;
            bubbleParent.transform.localScale = Vector3.Lerp(initialbubbleScale, Vector3.zero, lerpValue);
            yield return null;
        }

        currentTalkingCooldown = talkingCooldown + Random.Range(-0.5f, 0.5f);

        yield return new WaitForSeconds(currentTalkingCooldown);

        bubblePopCoroutine = StartCoroutine(TalkingAnimation());
    }

    public void StopTalking()
    {
        talking = false;
        bubbleParent.SetActive(false);

        if(bubblePopCoroutine != null)
            StopCoroutine(bubblePopCoroutine);

        bubbleParent.transform.localScale = initialbubbleScale;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, distanceToSeeConversation);
    }
}
