using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switchplatform : MonoBehaviour
{
    [SerializeField] private GameObject[] switchobjs;
    [SerializeField] private bool switchboth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject player = collision.gameObject;
            if (player.GetComponent<Playerstatemachine>().state == Playerstatemachine.States.Air || player.GetComponent<Playerstatemachine>().state == Playerstatemachine.States.Groundintoair)
            {
                if (Globalcalls.jumpcantriggerswitch == true)
                {
                    Globalcalls.jumpcantriggerswitch = false;
                    triggerswitch();
                }
            }
            else triggerswitch();
        }
    }
    private void triggerswitch()
    {
        foreach (GameObject obj in switchobjs)
        {
            if (obj.TryGetComponent(out Platformstate platformstate))
            {
                if (switchboth == true) platformstate.switchplatform();
                else platformstate.switchtored();
            }
        }
    }
}
