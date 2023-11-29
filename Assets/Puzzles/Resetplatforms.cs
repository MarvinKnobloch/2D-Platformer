using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetplatforms : MonoBehaviour
{
    [SerializeField] private GameObject[] resetplatforms;
    [SerializeField] private GameObject[] movementresetsandzones;
    private ParticleSystem particlesystem;

    private void Awake()
    {
        particlesystem = GetComponentInChildren<ParticleSystem>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            particlesystem.Stop();
            particlesystem.gameObject.SetActive(false);
            foreach (GameObject obj in resetplatforms)
            {
                if (obj.TryGetComponent(out Movingplatform movingplatform))
                {
                    if (movingplatform.moveonenter == true)
                    {
                        movingplatform.resetforminstant();
                    }
                }
                if (obj.TryGetComponent(out Platformstate platformstate))
                {
                    platformstate.resetswitchplatform();
                }
            }
            foreach (GameObject moveresets in movementresetsandzones)
            {
                moveresets.SetActive(true);
            }
            collision.GetComponent<Playerstatemachine>().abilitiesreset();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            particlesystem.gameObject.SetActive(true);
            particlesystem.Play();
        }
    }
}
