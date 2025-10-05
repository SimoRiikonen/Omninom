using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreable : MonoBehaviour
{
    [SerializeField] private int scoreValue = 1;
    private bool isScored = false;
    private float activeInterval = 0.0f;
    private float nextActionTime = 2.0f;
    private int baseScoreValue;

    public int ScoreValue
    {
        get { return scoreValue; }
    }

    public void SetPlayerScoreValue(int level)
    {
        scoreValue = baseScoreValue * level;
    }
    
    private void Start()
    {
        baseScoreValue = scoreValue;
    }

    private void Update()
    {
        activeInterval += Time.deltaTime;

        if (activeInterval >= nextActionTime)
        {
            if (transform.position.y < -30f)
                Destroy(gameObject);
        }
    }

    public void ScoreGiven(string playerId)
    {
        if (!isScored)
        {
            LevelManager.Instance.AddScoreForPlayer(playerId, scoreValue);
            isScored = true;
        }
    }

    public void ResetScore()
    {
        isScored = false;
    }
}
