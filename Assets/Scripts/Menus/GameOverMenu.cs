using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text titleLabel;
    [SerializeField] private TMP_Text coinLabel;
    [SerializeField] private Button mainMenuButton;

    public void Initialize(string titleText, string rewardText)
    {
        titleLabel.text = titleText;
        coinLabel.text = rewardText;
    }

    private void OnEnable()
    {
        mainMenuButton.onClick.AddListener(MainMenuClicked);
    }

    private void OnDisable()
    {
        mainMenuButton.onClick.RemoveListener(MainMenuClicked);
    }

    private void MainMenuClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
}
