using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookobject : MonoBehaviour
{
    public static List<GameObject> hookobjects = new List<GameObject>();
    private SpriteRenderer spriteRenderer;
    private float reactivatetimer;

    private void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hookobjects.Add(gameObject);
            reactivatetimer = 0;
            collision.gameObject.GetComponent<Playerstatemachine>().hooktargetupdate();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(hookobjects.Contains(gameObject) == false)
            {
                reactivatetimer += Time.deltaTime;
                if(reactivatetimer > 1.4f)
                {
                    hookobjects.Add(gameObject);
                    reactivatetimer = 0;
                    collision.gameObject.GetComponent<Playerstatemachine>().hooktargetupdate();
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            reactivatetimer = 0;
            hookobjects.Remove(gameObject);
            collision.gameObject.GetComponent<Playerstatemachine>().hooktargetupdate();
            spriteRenderer.color = Color.white;
        }
    }
}
