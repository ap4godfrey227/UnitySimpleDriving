using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private Button playButton;
    [SerializeField] private AndroidNotificationHandler androidNotificationHandler;
    [SerializeField] private int maxXpBoost;
    [SerializeField] private int boostRechargeDuration;
    

    private int xp;

    private const string XpKey = "XP Boost";
    private const string XpReadyKey = "XP Boost Ready";

    private void Start()
    {
        OnApplicationFocus(true);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if(!hasFocus) {return;}

        CancelInvoke();

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
            else
            {
                playButton.interactable = false;
                Invoke(nameof(xpRecharged), (xpReady - DateTime.Now).Seconds);
            }
        }

        xpText.text = $"Play\n XP Boost x{xp}";
    }

    private void xpRecharged()
    {
        playButton.interactable = true;
        xp = maxXpBoost;
        PlayerPrefs.SetInt(XpKey, xp);
        xpText.text = $"Play\n XP Boost x{xp}";
    }

    public void Play()
    {
        xp = PlayerPrefs.GetInt(XpKey, maxXpBoost);

        if(xp > 1)
        {
            xp--;
            PlayerPrefs.SetInt(XpKey, xp);
        }

        if(xp == 1)
        {
            DateTime resetTimeForXpBoost = DateTime.Now.AddMinutes(5);
            PlayerPrefs.SetString(XpReadyKey, resetTimeForXpBoost.ToString());
#if UNITY_ANDROID
            androidNotificationHandler.ScheduleNotification(resetTimeForXpBoost);
#endif  
        }
        

        SceneManager.LoadScene(1);
    }
}

