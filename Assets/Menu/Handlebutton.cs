using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Handlebutton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject menucontroller;
    [SerializeField] private GameObject menuoverview;
    [SerializeField] private GameObject settingscontroller;
    private Handlemenu handlemenu;

    private Color selectedcolor;
    private Color notselectedcolor;

    [SerializeField] private Menusoundcontroller menusoundcontroller;


    private void Awake()
    {
        handlemenu = GetComponentInParent<Handlemenu>();
        selectedcolor = handlemenu.selectedcolor;
        notselectedcolor = handlemenu.notselectedcolor;
    }
    private void OnEnable()
    {
        GetComponent<Image>().color = notselectedcolor;
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = selectedcolor;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = notselectedcolor;
    }
    public void resumegame()
    {
        menucontroller.GetComponent<Menucontroller>().closemenu();
    }
    public void opensettings()
    {
        settingscontroller.SetActive(true);
        menuoverview.SetActive(false);
        menusoundcontroller.playmenusound1();
    }
    public void closegame()
    {
        Application.Quit();
    }
    public void toggledash()
    {
        Globalcalls.candash = !Globalcalls.candash;
    }
}
