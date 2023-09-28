using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermemories
{
    public Playerstatemachine psm;
    public void playerplacememory()
    {
        if (psm.controlls.Player.Memorie.WasPerformedThisFrame())
        {
            if(psm.memoryisrunning == false && Globalcalls.currentmemorystacks > 0)
            {
                Globalcalls.currentmemorystacks--;
                Cooldowns.instance.handlememorystacks();
                psm.memoryisrunning = true;
                psm.memoryposition = psm.transform.position;
                psm.memoryvelocity = psm.rb.velocity;
                psm.playermemoryimage.transform.position = psm.transform.position;
                psm.playermemoryimage.transform.rotation = psm.transform.rotation;
                psm.playermemoryimage.SetActive(true);
                psm.memorycdobject.transform.parent.gameObject.SetActive(true);
                psm.memorycamera = psm.cinemachineConfiner.m_BoundingShape2D;
                return;
            }
            if (psm.memoryisrunning == true)
            {
                psm.memorycdobject.disablecd();              //called psm.endmemorytimer
                psm.rb.transform.position = psm.memoryposition;
                psm.rb.velocity = psm.memoryvelocity;
                psm.cinemachineConfiner.m_BoundingShape2D = psm.memorycamera;
                psm.switchtoairstate();
            }           
        }
    }
}
