using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Setresetpoint : MonoBehaviour
{
    private Vector3 resetposi;

    [SerializeField] private GameObject sectionobj;
    [SerializeField] private int camdistance;

    private void Awake()
    {
        sectionobj = GetComponentInParent<Sectioncamera>().sectiongameobj;
        camdistance = GetComponentInParent<Sectioncamera>().cameradistance;
        resetposi = transform.GetChild(0).transform.position;
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
