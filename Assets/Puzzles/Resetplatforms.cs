using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetplatforms : MonoBehaviour
{
    [SerializeField] private GameObject[] resetplatforms;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (GameObject obj in resetplatforms)
            {
                if (obj.TryGetComponent(out Platformstate platformstate))
                {
                    platformstate.resetswitchplatform();
                }
                if (obj.TryGetComponent(out Platformonentermove platformonentermove))
                {
                    platformonentermove.resetform();
                }
            }
        }
    }
}
