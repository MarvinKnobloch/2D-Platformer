using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetplatforms : MonoBehaviour
{
    [SerializeField] private GameObject[] resetplatforms;
    [SerializeField] private GameObject[] movementresets;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (GameObject obj in resetplatforms)
            {
                if (obj.TryGetComponent(out Movingplatform movingplatform))
                {
                    if(movingplatform.moveonenter == true)
                    {
                        movingplatform.resetforminstant();
                    }
                }
                if (obj.TryGetComponent(out Platformstate platformstate))
                {
                    platformstate.resetswitchplatform();
                }
            }
            foreach (GameObject moveresets in movementresets)
            {
                moveresets.SetActive(true);
            }
        }
    }
}
