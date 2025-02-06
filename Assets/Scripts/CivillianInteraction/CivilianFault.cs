using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CivilianFault : MonoBehaviour
{
    public List<CivilianInstance> _linkedCivilians;
    public bool censored = false;

    [SerializeField] private TalkingBubble talkingBubble;

    public SpriteRenderer spriteRenderer;

    [SerializeField] private Color censoredColor;

    [SerializeField] private UnityEvent customEvent;

    [field: Header("Runtime")]
    [field: SerializeField] public CivilianFaultType FaultType { get; private set; }

    public void Initialize(CivilianFaultType type)
    {
        talkingBubble.Initialize(type);
        FaultType = type;
    }


    public void Censor()
    {
        bool correctlyCensored = talkingBubble.badTopic || talkingBubble.badHat || talkingBubble.badBook || _linkedCivilians.Count > 4;
        if (correctlyCensored)
            spriteRenderer.color = censoredColor;
        else
            spriteRenderer.color = Color.green;

        FaultManager.Instance.ClearFault(talkingBubble._civilianFaultType);

        censored = true;
        Debug.Log("I've been censored!! Topic: " + talkingBubble.badTopic + " Hat " + talkingBubble.badHat + " Book " + talkingBubble.badBook);
        talkingBubble.StopTalking();

        customEvent.Invoke();

        OnCensored?.Invoke(correctlyCensored);

        
    }

    public event Action<bool> OnCensored;
}
