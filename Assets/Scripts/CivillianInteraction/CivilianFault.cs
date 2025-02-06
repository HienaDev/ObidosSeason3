using System;
using System.Collections.Generic;
using UnityEngine;

public class CivilianFault : MonoBehaviour
{
    public List<CivilianInstance> _linkedCivilians;
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
        bool correctlyCensored = talkingBubble.badTopic || talkingBubble.badHat || talkingBubble.badBook;
        if(correctlyCensored)
            spriteRenderer.color = censoredColor;
        else 
            spriteRenderer.color = Color.green;

        censored = true;
        Debug.Log("I've been censored!! Topic: " + talkingBubble.badTopic + " Hat " + talkingBubble.badHat + " Book " + talkingBubble.badBook);
        OnCensored?.Invoke(correctlyCensored);
    }

    public event Action<bool> OnCensored;
}
