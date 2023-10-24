using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Addmemoriestacks : MonoBehaviour
{
    public int stackcount;
    [SerializeField] private float memorymaxusetime;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(collision.gameObject.TryGetComponent(out Playerstatemachine playerstatemachine))
            {
                if(playerstatemachine.memoryisrunning == false)
                {
                    collision.gameObject.GetComponent<Playerstatemachine>().memorymaxusetime = memorymaxusetime;
                    Globalcalls.currentmemorystacks = stackcount;
                    Cooldowns.instance.displaymemoriestacks();
                }
            }
            gameObject.SetActive(false);
        }
    }
}
