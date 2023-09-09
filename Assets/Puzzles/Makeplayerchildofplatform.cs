using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Makeplayerchildofplatform : MonoBehaviour
{
    [SerializeField] private bool moveplatformmoveonenter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.parent = transform.parent;           //braucht ein übertransform damit der Scale vom Char nicht umgeändert wird
            if(moveplatformmoveonenter == true)
            {
                if(transform.parent.TryGetComponent(out Platformonentermove platformonentermove))
                {
                    if (platformonentermove.state == Platformonentermove.State.dontmove) platformonentermove.startmovement();
                }
            }
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
