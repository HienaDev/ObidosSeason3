using System;
using System.Collections;
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

    [SerializeField]
    private SpriteRenderer[] spriteRenderers;

    [SerializeField]
    private float timeToDisappear = 5f;
    [SerializeField]
    private float alphaReduceTime = 1f;

    private bool disappear = false;
    private Color tempColor;

    public void Initialize(CivilianFaultType type)
    {
        talkingBubble.Initialize(type);
        levelManager = FindAnyObjectByType<LevelManager>();
        FaultType = type;
    }

    private void FixedUpdate()
    {
        if (disappear)
        {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                tempColor = spriteRenderer.color;
                tempColor.a -= Time.fixedDeltaTime / alphaReduceTime;

                if(tempColor.a < 0f)
                {
                    tempColor.a = 0f;
                }

                spriteRenderer.color = tempColor;
            }
        }
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
        GetComponent<CircleCollider2D>().enabled = false;

        AudioSystem.PlaySound(cryClips);

        StartCoroutine(DisappearCoroutine());

        OnCensored?.Invoke(correctlyCensored);
    }

    public event Action<bool> OnCensored;

    private IEnumerator DisappearCoroutine()
    {
        yield return new WaitForSeconds(timeToDisappear);
        disappear = true;
    }
}
