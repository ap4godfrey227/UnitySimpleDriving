using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreMultiplier;

    public const string HighScoreKey = "HighScore";
    public const string xpTotalScoreKey = "xpScore";
    public const string LastScoreKey = "Last Score";

    private float score;

    // Update is called once per frame
    void Update()
    {
        score += Time.deltaTime * scoreMultiplier;

        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    private void OnDestroy() 
    {
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        PlayerPrefs.SetInt(LastScoreKey, Mathf.FloorToInt(score));

        if(score > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, Mathf.FloorToInt(score));
        }
        int currentXPBoost = PlayerPrefs.GetInt(MainMenu.scoreXpKey, 1);
        int currentTotalScore = PlayerPrefs.GetInt(xpTotalScoreKey, 0);
    
        PlayerPrefs.SetInt(xpTotalScoreKey, Mathf.FloorToInt((score*currentXPBoost)+currentTotalScore));
        
    }
}
