using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Addmemoriestacks : MonoBehaviour
{
    public int stackcount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Globalcalls.currentmemorystacks = stackcount;
            Cooldowns.instance.displaymemoriestacks();
        }
    }
}
