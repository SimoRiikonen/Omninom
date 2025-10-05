using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private IntermissionMenu intermissionMenu;
    [SerializeField] private GameOverMenu gameOverMenu;

    private void Start()
    {
        EventManager.StartListening(Events.GamePaused, OpenPauseMenu);
        EventManager.StartListening(Events.IntermissionActivated, OpenIntermissionMenu);
        EventManager.StartListening(Events.GameOver, OpenGameOverMenu);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.GamePaused, OpenPauseMenu);
        EventManager.StopListening(Events.IntermissionActivated, OpenIntermissionMenu);
        EventManager.StopListening(Events.GameOver, OpenGameOverMenu);
    }

    private void OpenPauseMenu(object data)
    {
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    private void OpenIntermissionMenu(object data)
    {
        intermissionMenu.Initialize(LevelManager.Instance.DropIntermissionReward());
        intermissionMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    private void OpenGameOverMenu(object data)
    {
        var castedData = (string[])data;
        gameOverMenu.Initialize(castedData[0], castedData[1]);
        gameOverMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
}
