using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Setresetpoint : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private Vector3 resetposi;

    [SerializeField] private GameObject sectionobj;
    [SerializeField] private int camdistance;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(1, 6);
        boxCollider2D.offset = new Vector2(0, 3.5f);

        resetposi = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        sectionobj = GetComponentInParent<Sectioncamera>().sectiongameobj;
        camdistance = GetComponentInParent<Sectioncamera>().cameradistance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Globalcalls.playeresetpoint = resetposi;
            Globalcalls.boundscolliderobj = sectionobj;
            Globalcalls.savecameradistance = camdistance;
        }
            
    }
}
