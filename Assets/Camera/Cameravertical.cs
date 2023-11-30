using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Cameravertical : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D leftcollider;
    [SerializeField] private PolygonCollider2D rightcollider;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private CinemachineConfiner cinemachineConfiner;
    private Collider2D Collider2D;

    [SerializeField] private float leftcameradistance;
    [SerializeField] private float rightcameradistance;

    [SerializeField] private Musiccontroller musiccontroller;
    [SerializeField] private AudioClip leftsidesong;
    [Range(0.1f , 1)]
    [SerializeField] private float leftsidevolume;
    [SerializeField] private AudioClip rightsidesong;
    [Range(0.1f, 1)]
    [SerializeField] private float rightsidevolume;

    private void Awake()
    {
        Collider2D = GetComponent<Collider2D>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 exitdirection = (collision.transform.position - Collider2D.bounds.center).normalized;
            if (exitdirection.x < 0)
            {
                cinemachineConfiner.m_BoundingShape2D = leftcollider;
                cinemachineVirtualCamera.m_Lens.OrthographicSize = leftcameradistance;
                if(musiccontroller != null)
                {
                    musiccontroller.startfadeout(leftsidesong, 0, 1, 1, leftsidevolume);
                }
            }
            else 
            { 
                cinemachineConfiner.m_BoundingShape2D = rightcollider;
                cinemachineVirtualCamera.m_Lens.OrthographicSize = rightcameradistance;
                if (musiccontroller != null)
                {
                    musiccontroller.startfadeout(rightsidesong, 0, 1, 1, rightsidevolume);
                }
            }
            
        }
    }

}
