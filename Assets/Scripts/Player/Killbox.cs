using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killbox : MonoBehaviour
{
    private string playerId;
    public string PlayerId => playerId;

    public void SetPlayerId(string id)
    {
        playerId = id;
    }
}