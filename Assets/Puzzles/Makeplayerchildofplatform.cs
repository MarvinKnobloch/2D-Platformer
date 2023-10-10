using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Makeplayerchildofplatform : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(transform.GetChild(0).transform.localScale.x * 0.96f, transform.GetChild(0).transform.localScale.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out Playerstatemachine playerstatemachine))
            {
                if (gameObject.TryGetComponent(out Movingplatform movingplatform))
                {
                    if(playerstatemachine.gravityswitchactiv == false) collision.GetComponent<Rigidbody2D>().gravityScale = 30;
                    else collision.GetComponent<Rigidbody2D>().gravityScale = -30;
                    playerstatemachine.movingplatform = movingplatform;
                    playerstatemachine.isonplatform = true;
                    if (movingplatform.moveonenter == true)
                    {
                        if (movingplatform.state == Movingplatform.State.dontmove) movingplatform.startmovement();
                    }

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
                if(playerstatemachine.movingplatform != null)
                {
                    if (playerstatemachine.movingplatform.gameObject == gameObject)
                    {
                        playerstatemachine.isonplatform = false;
                        playerstatemachine.movingplatform = null;
                    }
                }
            }
        }
    }
}
