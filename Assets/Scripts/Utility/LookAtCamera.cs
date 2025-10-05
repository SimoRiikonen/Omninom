using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void Start()
    {
        transform.localScale = new Vector3(-0.01f, 0.01f, 0.01f); 
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
    }
}
