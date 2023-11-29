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
                psm.memorydashcount = psm.currentdashcount;
                psm.playermemoryimage.transform.position = psm.transform.position;
                psm.playermemoryimage.transform.rotation = psm.transform.rotation;
                psm.playermemoryimage.SetActive(true);
                psm.memorycdobject.transform.parent.gameObject.SetActive(true);
                psm.memorycamera = psm.cinemachineConfiner.m_BoundingShape2D;
                psm.playermemorysound.playmemory();
                return;
            }
            if (psm.memoryisrunning == true)
            {
                psm.memorystart();
            }           
        }
    }
    //IEnumerator usememory()
    //{
    //    psm.state = Playerstatemachine.States.Empty;
    //    psm.rb.transform.position = psm.memoryposition;
    //    psm.rb.velocity = Vector2.zero;
    //    yield return new WaitForSeconds(0.1f);
    //    psm.state = Playerstatemachine.States.Air;
    //    psm.memorycdobject.disablecd();              //called psm.endmemorytimer
    //    psm.rb.velocity = psm.memoryvelocity;
    //    psm.currentdashcount = psm.memorydashcount;
    //    psm.cinemachineConfiner.m_BoundingShape2D = psm.memorycamera;
    //    psm.switchtoairstate();
    //}
}
