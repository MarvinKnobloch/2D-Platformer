using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerresetzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(Globalcalls.playeresetpoint != Vector3.zero)
            {
                collision.gameObject.transform.position = Globalcalls.playeresetpoint;
                collision.gameObject.GetComponent<Playerstatemachine>().resetplayer();
            }
        }
    }
}
