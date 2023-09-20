using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Addgravitystacks : MonoBehaviour
{
    public int stackcount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Globalcalls.currentgravitystacks = stackcount;
            Cooldowns.instance.displaygravitystacks();
        }
    }
}
