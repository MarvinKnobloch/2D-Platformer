using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Cameravertical : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D leftcollider;
    [SerializeField] private PolygonCollider2D rightcollider;
    [SerializeField] private CinemachineConfiner cinemachineConfiner;
    private Collider2D Collider2D;

    private void Awake()
    {
        Collider2D = GetComponent<Collider2D>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 exitdirection = (collision.transform.position - Collider2D.bounds.center).normalized;
            if (exitdirection.x < 0) cinemachineConfiner.m_BoundingShape2D = leftcollider;
            else cinemachineConfiner.m_BoundingShape2D = rightcollider;

        }
    }

}
