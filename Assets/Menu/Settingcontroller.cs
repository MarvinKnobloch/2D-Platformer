using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settingcontroller : MonoBehaviour
{
    private Controlls controlls;

    [SerializeField] private GameObject menuoverview;
    [SerializeField] private Menusoundcontroller menusoundcontroller;

    private void Awake()
    {
        controlls = Keybindinputmanager.inputActions;
    }
    private void OnEnable()
    {
        controlls.Enable();
    }
    private void Update()
    {
        if (controlls.Menu.Openmenu.WasPerformedThisFrame())
        {
            menuoverview.SetActive(true);
            gameObject.SetActive(false);
            menusoundcontroller.playmenusound1();
        }
    }
}
