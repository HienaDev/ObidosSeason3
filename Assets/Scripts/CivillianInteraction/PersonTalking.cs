using UnityEngine;

public class PersonTalking : MonoBehaviour
{

    public bool censored = false;

    [SerializeField] private TalkingBubble talkingBubble;

    public SpriteRenderer spriteRenderer;

    [SerializeField] private Color censoredColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
    }


    public void Censor()
    {
        if(talkingBubble.badTopic || talkingBubble.badHat || talkingBubble.badBook)
            spriteRenderer.color = censoredColor;
        else 
            spriteRenderer.color = Color.green;

        censored = true;
        Debug.Log("I've been censored!! Topic: " + talkingBubble.badTopic + " Hat " + talkingBubble.badHat + " Book " + talkingBubble.badBook);
    }
}
