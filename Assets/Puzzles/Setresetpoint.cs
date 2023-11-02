using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Setresetpoint : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner cinemachineConfiner;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Globalcalls.playeresetpoint = collision.gameObject.transform.position;
            Globalcalls.boundscolliderobj = cinemachineConfiner.m_BoundingShape2D.gameObject;
        }
            
    }
}
