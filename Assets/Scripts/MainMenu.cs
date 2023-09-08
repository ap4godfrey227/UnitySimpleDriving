using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private int maxXpBoost;
    [SerializeField] private int boostRechargeDuration;
    

    private int xp;

    private const string XpKey = "XP Boost";
    private const string XpReadyKey = "XP Boost Ready";

    private void Start()
    {
        int highScore = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);

        highScoreText.text = "High Score: " + highScore.ToString();

        xp = PlayerPrefs.GetInt(XpKey, maxXpBoost);

        if(xp == 1)
        {
            string xpReadyString = PlayerPrefs.GetString(XpReadyKey, string.Empty);

            if(xpReadyString == string.Empty){ return;}

            DateTime xpReady = DateTime.Parse(xpReadyString);

            if(DateTime.Now > xpReady)
            {
                xp = maxXpBoost;
                PlayerPrefs.SetInt(XpKey, xp);
            }
        }

        xpText.text = $"Play/n XP Boost x{xp}";
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
