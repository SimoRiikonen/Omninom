using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillboxObject : Killbox
{
    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(PlayerId))
        {
            if (!other.CompareTag(TagNames.Object))
            {
                return;
            }

            other.GetComponent<Rigidbody>().mass = 1000f;
            other.GetComponent<Scoreable>().ScoreGiven(PlayerId);

            EventManager.TriggerEvent(Events.ObjectDestroyed, other.gameObject);
        }
    }
}
