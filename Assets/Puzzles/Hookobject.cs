using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookobject : MonoBehaviour
{
    public static List<GameObject> hookobjects = new List<GameObject>();
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hookobjects.Add(gameObject);
            spriteRenderer.color = Color.green;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hookobjects.Remove(gameObject);
            spriteRenderer.color = Color.red;
        }
    }
}
