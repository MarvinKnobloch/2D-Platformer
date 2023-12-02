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
        //gravityhotkey = controls.Player.Gravityswitch.GetBindingDisplayString();
        //tutorialtext.text = "\nAfter stepping in the green light press <color=green>" + gravityhotkey + "</color> to switch gravity.\n" +
        //                    "The number on the bottom left will display how often you can use this ability.";


        gravityhotkey = controls.Player.Gravityswitch.GetBindingDisplayString();
        tutorialtext.text = "Step into the green light to gain the gravity switch ability.\n" +
                            "\nPress <color=green>" + gravityhotkey + "</color> to use it.\n" +
                            "\nThe icon on the bottom left will display how often you can switch gravtiy.";
    }
}
