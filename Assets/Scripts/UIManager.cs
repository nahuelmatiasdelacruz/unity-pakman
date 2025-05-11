using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager sharedInstance;
    public TextMeshProUGUI titleLabel;
    public TextMeshProUGUI scoreLabel;
    private int totalScore = 0;

    private void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    private void Update()
    {
        if(GameManager.sharedInstance.gamePaused || !GameManager.sharedInstance.gameStarted)
        {
            titleLabel.enabled = true;
        }
        else
        {
            titleLabel.enabled = false;
        }
    }

    public void IncreaseScore(int score)
    {
        totalScore += score;
        scoreLabel.text = "Score: " + totalScore;
    }

}
