using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColliderController : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    private List<string> playersWithClaim = new List<string>();

    public void ToggleCollider(bool isEnabled, string playerId)
    {
        if (myCollider != null)
        {
            if (isEnabled)
            {
                playersWithClaim.Remove(playerId);

                if (playersWithClaim.Count == 0)
                {
                    myCollider.enabled = true;
                    EventManager.StopListening(Events.PlayerKilled, OnPlayerKilled);
                }
            }
            else
            {
                if (!playersWithClaim.Contains(playerId))
                {
                    playersWithClaim.Add(playerId);
                }

                myCollider.enabled = false;
                EventManager.StartListening(Events.PlayerKilled, OnPlayerKilled);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (myCollider.enabled)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }

    private void OnPlayerKilled(object data)
    {
        string playerId = (string)data;

        if (playersWithClaim.Contains(playerId))
        {
            playersWithClaim.Remove(playerId);

            if (playersWithClaim.Count == 0)
            {
                myCollider.enabled = true;
            }
        }
    }
}
