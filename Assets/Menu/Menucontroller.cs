using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menucontroller : MonoBehaviour
{
    private Controlls controlls;

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject menuoverview;
    private float normaltimescale;
    private float normalfixeddeltatime;

    private void Awake()
    {
        controlls = Keybindinputmanager.inputActions;
        normaltimescale = Time.timeScale;
        normalfixeddeltatime = Time.fixedDeltaTime;
        Globalcalls.gameispaused = false;
    }
    private void OnEnable()
    {
        controlls.Enable();
    }
    private void Update()
    {
        if (controlls.Menu.Openmenu.WasPerformedThisFrame() && Globalcalls.gameispaused == false)
        {
            Globalcalls.gameispaused = true;
            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0f;
            menu.SetActive(true);
            menuoverview.SetActive(true);
        }
        else if(controlls.Menu.Openmenu.WasPerformedThisFrame() && menuoverview.activeSelf == true && menu.activeSelf == true)     //menu.activeSelf == true weil menuoverview als aktiv gilt selbst wenn der parent disabled ist
        {
            closemenu();
        }
    }
    public void closemenu()
    {
        menuoverview.SetActive(false);
        Globalcalls.gameispaused = false;
        Time.timeScale = normaltimescale;
        Time.fixedDeltaTime = normalfixeddeltatime;
        menu.SetActive(false);
    }
}
