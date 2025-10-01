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

    private LevelManager levelManager;

    [field: Header("Runtime")]
    [field: SerializeField] public CivilianFaultType FaultType { get; private set; }

    [SerializeField] public bool menuButton = false;

    [SerializeField] private AudioClip[] cryClips;

    public void Initialize(CivilianFaultType type)
    {
        talkingBubble.Initialize(type);
        levelManager = FindAnyObjectByType<LevelManager>();
        FaultType = type;
    }


    public void Censor()
    {

        if(menuButton)
        {
            customEvent.Invoke();
            return;
        }

        bool correctlyCensored = talkingBubble.badSinging || talkingBubble.badRadio || talkingBubble.badTopic || talkingBubble.badHat || talkingBubble.badBook || _linkedCivilians.Count > 4;
        if (correctlyCensored)
            spriteRenderer.color = censoredColor;
        else
        {
            if(!talkingBubble.topicManager.badgeCover1.activeSelf)
                talkingBubble.topicManager.badgeCover1.SetActive(true);
            else if (!talkingBubble.topicManager.badgeCover2.activeSelf)
                talkingBubble.topicManager.badgeCover2.SetActive(true);
            else
            {
                // Restart Day
                levelManager.RestartDay(true);
            }
        }

        FaultManager.Instance.ClearFault(talkingBubble._civilianFaultType);

        censored = true;
        //Debug.Log("I've been censored!! Topic: " + talkingBubble.badTopic + " Hat " + talkingBubble.badHat + " Book " + talkingBubble.badBook);
        talkingBubble.StopTalking();

        AudioSystem.PlaySound(cryClips);

        OnCensored?.Invoke(correctlyCensored);

        
    }

    public event Action<bool> OnCensored;
}
