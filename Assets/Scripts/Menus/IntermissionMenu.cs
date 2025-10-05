using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntermissionMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text rewardLabel;
    [SerializeField] private Button continueButton;

    public void Initialize(int rewardAmount)
    {
        var rewardText = string.Concat("You gained: ", rewardAmount.ToString(), " coins!");
        LevelManager.Instance.AddCoinsForMainPlayer(rewardAmount);
        rewardLabel.text = rewardText;
    }

    private void OnEnable()
    {
        continueButton.onClick.AddListener(ContinueGame);
    }

    private void OnDisable()
    {
        continueButton.onClick.RemoveListener(ContinueGame);
    }

    private void ContinueGame()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
