using System.Collections;
using UnityEngine;

public class TalkingBubble : MonoBehaviour
{

    private CreateTopics topicManager;
    private Sprite topicSprite;
    private Transform player;
    [SerializeField] private GameObject bubbleParent;
    [SerializeField] private float distanceToSeeConversation = 5f;
    [SerializeField] private Sprite cantSeeSprite;
    [SerializeField] private SpriteRenderer symbolPlace;

    private bool talking = false;
    //private bool topicIsFarAway = true;
    public bool badTopic = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        topicManager = FindAnyObjectByType<CreateTopics>();
        player = FindAnyObjectByType<PlayerMovement>().gameObject.transform;

        yield return new WaitForSeconds(0.1f);
        StartTalking();
    }

    private void FixedUpdate()
    {
        if (talking)
            if (Vector2.Distance(player.position, transform.position) > distanceToSeeConversation)// && !topicIsFarAway)
            {
                if (cantSeeSprite != null)
                {
                    symbolPlace.sprite = cantSeeSprite;
                }
                //topicIsFarAway = true;
            }
            else if (Vector2.Distance(player.position, transform.position) < distanceToSeeConversation)// && topicIsFarAway)
            {
                if (topicSprite != null)
                {
                    symbolPlace.sprite = topicSprite;
                }
                //topicIsFarAway = false;
            }
    }

    public void StartTalking()
    {
        talking = true;
        bubbleParent.SetActive(true);
        (topicSprite, badTopic) = topicManager.GetRandomTopic();
        Debug.Log("talking: " + topicSprite.name);
    }
}
