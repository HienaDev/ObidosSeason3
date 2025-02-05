using UnityEngine;

public class PersonTalking : MonoBehaviour
{

    public bool censored = false;

    private TalkingBubble talkingBubble;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        talkingBubble = GetComponentInChildren<TalkingBubble>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Censor()
    {
        if(talkingBubble.badTopic || talkingBubble.badHat || talkingBubble.badBook)
            GetComponent<SpriteRenderer>().color = Color.blue;
        else 
            GetComponent<SpriteRenderer>().color = Color.green;

        censored = true;
        Debug.Log("I've been censored!!");
    }
}
