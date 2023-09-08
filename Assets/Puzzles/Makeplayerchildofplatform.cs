using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Makeplayerchildofplatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = transform.parent;           //braucht ein �bertransform damit der Scale vom Char nicht umge�ndert wird
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
