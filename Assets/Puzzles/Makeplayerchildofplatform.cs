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
        float xsize = transform.GetChild(0).GetComponent<SpriteRenderer>().size.x;
        boxCollider.size = new Vector2(xsize * 0.49f, transform.GetChild(0).transform.localScale.y);  // 0.96f
        transform.GetChild(0).GetComponent<BoxCollider2D>().size = new Vector2(xsize, 1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out Playerstatemachine playerstatemachine))
            {
                if (gameObject.TryGetComponent(out Movingplatform movingplatform))
                {
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
                        controllgravity(playerstatemachine);
                    }
                }
            }
        }
    }
    private void controllgravity(Playerstatemachine playerstatemachine)
    {
        if (playerstatemachine.state == Playerstatemachine.States.Ground)
        {
            if (playerstatemachine.gravityswitchactiv == false) playerstatemachine.rb.gravityScale = playerstatemachine.groundgravityscale;
            else playerstatemachine.rb.gravityScale = playerstatemachine.groundgravityscale * -1;
        }
    }
}
