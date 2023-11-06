using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Gravitytutorial : MonoBehaviour
{
    private TextMeshProUGUI tutorialtext;
    private Controlls controls;

    private string gravityhotkey;

    private void Awake()
    {
        tutorialtext = GetComponent<TextMeshProUGUI>();
        controls = Keybindinputmanager.inputActions;
        hotkeysandtextupdate();
    }
    private void OnEnable()
    {
        Menucontroller.tutorialupdate += hotkeysandtextupdate;
    }
    private void OnDisable()
    {
        Menucontroller.tutorialupdate -= hotkeysandtextupdate;
    }

    private void hotkeysandtextupdate()
    {
        gravityhotkey = controls.Player.Gravityswitch.GetBindingDisplayString();
        tutorialtext.text = "Step on the \"S\" button to the save your postion.\n" +
                            "Black zone = death zone and you will be ported back to your last save postion.\n" +
                            "\nAfter stepping on the \"G\" press <color=green>" + gravityhotkey + "</color> to switch gravity.\n" +
                            "The number on the bottom left will display how often you can this ability.";
    }
}
