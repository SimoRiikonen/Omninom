using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MovePlayer(Vector3 direction)
    {
        if (rb == null)
        {
            return;
        }

        if (direction.magnitude > 0)
        {
            rb.velocity = direction.normalized * speed; 
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}