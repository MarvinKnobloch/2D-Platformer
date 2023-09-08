using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Makeplayerchildofplatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = transform.parent;           //braucht ein übertransform damit der Scale vom Char nicht umgeändert wird
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}
