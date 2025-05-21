using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int timeForLevelToComplete = 60;
    public int tempTime;
    public Image timeImage;
    public TMP_Text timeText;
    public TMP_Text scoreText;
    public TMP_Text TurnText;
    Coroutine timer;
    public int score;
    public int turns;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        timer = StartCoroutine(Timer());
        // StartCoroutine("Timer");
        AddScore(0);
    }

    IEnumerator Timer()
    {
        tempTime = timeForLevelToComplete;
        while (tempTime > 0)
        {
            tempTime--; // current time
            yield return new WaitForSeconds(1);

            timeImage.fillAmount = tempTime / (float)timeForLevelToComplete; // currentTime/maxTime
            // fill anount (0,1) so we use slash /
            timeText.text = tempTime.ToString();
        }
        // Game Over
        GameManager.instance.GameOver();
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = score.ToString("D4"); // number has 8 decimal length
    }
    public void AddTurn()
    {
        turns++;
        TurnText.text = turns.ToString("D2");
    }

    public void StopTimer()
    {
        StopCoroutine(timer);
    }

}
