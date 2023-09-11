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
    [SerializeField] private TMP_Text xpTitle;
    [SerializeField] private TMP_Text xpLevel;
    [SerializeField] private TMP_Text LastRun;
    [SerializeField] private Slider xpBar;
    [SerializeField] private Button playButton;
    [SerializeField] private AndroidNotificationHandler androidNotificationHandler;
    [SerializeField] private int maxXpBoost;
    [SerializeField] private int boostRechargeDuration;
    
    

    private int xp;

    public const string XpKey = "XP Boost";
    public const string scoreXpKey = "score xp";
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
                Invoke(nameof(xpRecharged), (xpReady - DateTime.Now).Seconds);
            }
        }

        xpText.text = $"Play\n XP Boost x{xp}";

        //xp Leveling

        int totalScore = PlayerPrefs.GetInt(ScoreSystem.xpTotalScoreKey, 0);
        int lastScore = PlayerPrefs.GetInt(ScoreSystem.LastScoreKey, 0);
        xpLevel.text = $"{totalScore%1000}/1000";
        xpTitle.text = $"XP Level:{Mathf.FloorToInt(totalScore/1000)}";
        LastRun.text = $"Previous Score:{lastScore}";
        float sliderValue = (totalScore%1000)*0.001f;
        xpBar.value = sliderValue;
    }

    private void xpRecharged()
    {
        xp = maxXpBoost;
        PlayerPrefs.SetInt(XpKey, xp);
        xpText.text = $"Play\n XP Boost x{xp}";
    }

    public void Play()
    {
        xp = PlayerPrefs.GetInt(XpKey, maxXpBoost);
        PlayerPrefs.SetInt(scoreXpKey, xp);

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

