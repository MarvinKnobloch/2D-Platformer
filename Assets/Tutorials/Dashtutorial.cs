using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashtutorial : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Globalcalls.candash = true;
        }
    }
}
