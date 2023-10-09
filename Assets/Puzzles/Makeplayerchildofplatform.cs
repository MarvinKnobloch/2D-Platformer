using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Makeplayerchildofplatform : MonoBehaviour
{
    [SerializeField] private bool moveplatformmoveonenter;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //collision.transform.parent = transform;        //braucht ein übertransform damit der Scale vom Char nicht umgeändert wird
            collision.GetComponent<Playerstatemachine>().isonplatform = true;
            collision.GetComponent<Rigidbody2D>().gravityScale = 30;
            collision.GetComponent<Playerstatemachine>().platformrb = rb;

            if (moveplatformmoveonenter == true)
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
            if(collision.TryGetComponent(out Playerstatemachine playerstatemachine))
            {
                if(playerstatemachine.platformrb.gameObject == gameObject)
                {
                    playerstatemachine.isonplatform = false;
                    playerstatemachine.platformrb = null;
                }
            }
            //if (collision.transform.IsChildOf(gameObject.transform))
            //{
            //    collision.GetComponent<Playerstatemachine>().isonplatform = false;
            //    collision.GetComponent<Playerstatemachine>().platformrb = null;
            //    collision.transform.parent = null;
            //}
        }
    }
}
