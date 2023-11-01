using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Scoutcontroller : MonoBehaviour
{
    private Controlls controlls;
    [SerializeField] private GameObject scoutobj;
    [SerializeField] private GameObject menu;

    [SerializeField] private GameObject scouttarget;
    [SerializeField] private GameObject player;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtual;
    private void Awake()
    {
        controlls = Keybindinputmanager.inputActions;
    }
    void Update()
    {
        if (controlls.Player.Scoutmode.WasPerformedThisFrame())
        {
            if(menu.activeSelf == false)
            {
                if (Globalcalls.gameispaused == false)
                {
                    Globalcalls.gameispaused = true;
                    scoutobj.SetActive(true);
                    scouttarget.transform.position = player.transform.position;
                    cinemachineVirtual.Follow = scouttarget.transform;
                    return;
                }
                else if (scoutobj.activeSelf == true) StartCoroutine("closescoutmode");
            }
        }
        if (controlls.Menu.Openmenu.WasPerformedThisFrame())
        {
            if (menu.activeSelf == false && scoutobj.activeSelf == true) StartCoroutine("closescoutmode");
        }
    }
    IEnumerator closescoutmode()
    {
        yield return null;
        Globalcalls.gameispaused = false;
        scoutobj.SetActive(false);
        cinemachineVirtual.Follow = player.transform;
    }
}
