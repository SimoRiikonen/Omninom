using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillboxPlayer : Killbox
{
    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(PlayerId))
        {
            if (!other.CompareTag(TagNames.Player))
            {
                return;
            }

            var otherPlayer = other.GetComponent<Player>();

            if (otherPlayer == null)
            {
                return;
            }

            if (otherPlayer.PlayerLevel < GetComponentInParent<Player>().PlayerLevel)
            {
                otherPlayer.PlayerKilled();
                other.GetComponent<Scoreable>().ScoreGiven(PlayerId);
            }
        }
    }
}