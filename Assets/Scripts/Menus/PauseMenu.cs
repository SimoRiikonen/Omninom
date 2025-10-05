using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button  quitButton;

    private void OnEnable()
    {
        continueButton.onClick.AddListener(ContinueGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void OnDisable()
    {
        continueButton.onClick.RemoveListener(ContinueGame);
        quitButton.onClick.RemoveListener(QuitGame);
    }

    private void ContinueGame()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
