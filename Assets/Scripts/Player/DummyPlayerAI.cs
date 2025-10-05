using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayerAI : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    private Vector3 targetPosition;

    private float activeInterval = 0.0f;
    private float nextActionTime = 5.0f;

    private void OnValidate()
    {
        playerMovement = GetComponent<PlayerMovement>();

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement-component not found.");
        }
    }

    private void Start()
    {
        targetPosition = LevelManager.Instance.GetTargetPositionForDummyPlayer();
    }

    private void Update()
    {
        activeInterval += Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
        {
            targetPosition = LevelManager.Instance.GetTargetPositionForDummyPlayer();
        }
        else if (activeInterval >= nextActionTime)
        {
            targetPosition = LevelManager.Instance.GetTargetPositionForDummyPlayer();
            activeInterval = 0.0f;
        }

        playerMovement.MovePlayer(targetPosition - transform.position);
    }
}
