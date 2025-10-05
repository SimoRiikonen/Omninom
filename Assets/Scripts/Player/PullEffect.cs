using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullEffect : MonoBehaviour
{
    [SerializeField] private float pullForce = 2f;
    [SerializeField] private float pullRadius = 2f;

    private string playerId;

    private void FixedUpdate()
    {
        var colliders = Physics.OverlapSphere(transform.position, pullRadius);

        foreach (Collider collider in colliders)
        {       
            if (collider.CompareTag(TagNames.Object))
            {
                if (Vector3.Distance(collider.transform.position, transform.position) > 0.2f)
                {
                    var rb = collider.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(-pullForce, transform.position, pullRadius);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagNames.Tile))
        {
            TileColliderController tileCollider = other.GetComponent<TileColliderController>();
            if (tileCollider != null)
            {
                tileCollider.ToggleCollider(false, playerId);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagNames.Tile))
        {
            TileColliderController tileCollider = other.GetComponent<TileColliderController>();
            if (tileCollider != null)
            {
                tileCollider.ToggleCollider(true, playerId);
            }
        }
    }

    public void SetPlayerId(string id)
    {
        playerId = id;
        this.enabled = true;
    }

    public void SetEffectValues(float force, float radius)
    {
        pullForce = force;
        pullRadius = radius;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}
