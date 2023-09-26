using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camerabounds : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner cinemachineConfiner;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cameraswitch"))
        {
            cinemachineConfiner.m_BoundingShape2D = GetComponent<PolygonCollider2D>();
        }
    }

}
