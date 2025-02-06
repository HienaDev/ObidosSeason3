using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Timer : MonoBehaviour
{
    private float timer = 0f;

    private TextMeshProUGUI timerText;

    private bool gameStart;
    private bool gameEnd;

    [SerializeField] private GameObject player;

    private int minute;

    [SerializeField] private GameObject leaderboard;

    public int TimeFinal { get { return (int)((minute * 60) + timer); } }




    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        timerText.text = "00 : 00";

        gameStart = false;
        gameEnd = false;

        minute = 0;
    }

    private void Update()
    {
        if (player != null && player.transform.position.y > 240)
            gameStart = true;

        //Debug.Log(timer);

        if (gameStart)
        {
            timer -= Time.deltaTime;

            if (timer > 60f)
            {
                timer = 0;
                minute++;
            }


            if (minute < 10f)
            {
                if (timer < 10f)
                    timerText.text = string.Format("0{0} : 0{1}", minute, Math.Truncate(timer));
                else
                    timerText.text = string.Format("0{0} : {1}", minute, Math.Truncate(timer));
            }
            else
            {
                if (timer < 10f)
                    timerText.text = string.Format("{0} : 0{1}", minute, Math.Truncate(timer));
                else
                    timerText.text = string.Format("{0} : {1}", minute, Math.Truncate(timer));
            }
        }
    }

    public void StartTimer(float seconds)
    {
        timer = seconds;
        gameStart = true;
    }

    public void EndGame()
    {
        gameEnd = true;
        timerText.color = new Color32(0, 255, 0, 255);

        Debug.Log(TimeFinal);

        leaderboard.SetActive(true);
    }
}
