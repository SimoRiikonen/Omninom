using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerStatusDisplay : MonoBehaviour
{
    [SerializeField] private Image levelProgressBar;
    [SerializeField] private TMP_Text levelLabel;
    [SerializeField] private TMP_Text progressLabel;

    public void UpdateLevelDisplay(int level)
    {
        levelLabel.text = level.ToString();
    }

    public void UpdateLevelProgress(int progress, int target)
    {
        var fillAmount = (float)progress / (float)target;
        
        levelProgressBar.fillAmount = fillAmount;
        progressLabel.text = string.Concat(progress.ToString(), " / ", target.ToString());
    }
}
