using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button smallMapButton;
    [SerializeField] private Button largeMapButton;
    [SerializeField] private TMP_Text coinLabel;

    private void OnEnable()
    {
        var playerCoinAmount = PlayerPrefs.GetInt("PlayerCoinCount", 0);
        coinLabel.text = string.Concat("Coins: ", playerCoinAmount.ToString());

        smallMapButton.onClick.AddListener(PlaySmallButtonClicked);
        largeMapButton.onClick.AddListener(PlayLargeButtonClicked);
    }

    private void OnDisable()
    {
        smallMapButton.onClick.RemoveListener(PlaySmallButtonClicked);
        largeMapButton.onClick.RemoveListener(PlayLargeButtonClicked);
    }

    private void PlaySmallButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay1");
    }

    private void PlayLargeButtonClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay2");
    }
}
