using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Addmemoriestacks : MonoBehaviour
{
    public int stackcount;
    [SerializeField] private float memorymaxusetime;

    [SerializeField] private bool memoryandgravity;
    [SerializeField] private int gravitystackcount;
    [SerializeField] private bool triggerwhennormalgravity;
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
                if(memoryandgravity == true)
                {
                    if (triggerwhennormalgravity == true)
                    {
                        if (collision.gameObject.GetComponent<Playerstatemachine>().gravityswitchactiv == false)
                        {
                            Globalcalls.currentgravitystacks = gravitystackcount;
                            Cooldowns.instance.displaygravitystacks();
                        }
                    }
                    else
                    {
                        if (collision.gameObject.GetComponent<Playerstatemachine>().gravityswitchactiv == true)
                        {
                            Globalcalls.currentgravitystacks = gravitystackcount;
                            Cooldowns.instance.displaygravitystacks();
                        }
                    }
                }
            }
            gameObject.SetActive(false);
        }
    }
}
