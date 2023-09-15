using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setresetpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Globalcalls.playeresetpoint = collision.gameObject.transform.position;
        }
            
    }
}
