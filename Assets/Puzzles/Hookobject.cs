using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookobject : MonoBehaviour
{
    public static List<GameObject> hookobjects = new List<GameObject>();
    private SpriteRenderer spriteRenderer;
    private float reactivatetimer;
    private GameObject childobj;

    private void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        childobj = transform.GetChild(0).gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hookobjects.Add(childobj);
            reactivatetimer = 0;
            collision.gameObject.GetComponent<Playerstatemachine>().hooktargetupdate();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(hookobjects.Contains(childobj) == false)
            {
                reactivatetimer += Time.deltaTime;
                if(reactivatetimer > 1.4f)
                {
                    hookobjects.Add(childobj);
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
            hookobjects.Remove(childobj);
            collision.gameObject.GetComponent<Playerstatemachine>().hooktargetupdate();
            spriteRenderer.color = Color.white;
        }
    }
}
