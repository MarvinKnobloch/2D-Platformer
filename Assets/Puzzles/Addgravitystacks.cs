using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Addgravitystacks : MonoBehaviour
{
    public int stackcount;
    [SerializeField] private bool triggerwhennormalgravity;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(triggerwhennormalgravity == true)
            {
                if (collision.gameObject.GetComponent<Playerstatemachine>().gravityswitchactiv == false)
                {
                    Globalcalls.currentgravitystacks = stackcount;
                    Cooldowns.instance.displaygravitystacks();
                }
            }
            else
            {
                if (collision.gameObject.GetComponent<Playerstatemachine>().gravityswitchactiv == true)
                {
                    Globalcalls.currentgravitystacks = stackcount;
                    Cooldowns.instance.displaygravitystacks();
                }
            }
        }
    }
}
