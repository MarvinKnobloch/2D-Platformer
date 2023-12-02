using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Dashtutorial : MonoBehaviour
{
    private TextMeshProUGUI tutorialtext;
    private Controlls controls;

    private string dashhotkey;

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
        dashhotkey = controls.Player.Dash.GetBindingDisplayString();
        tutorialtext.text = "Press <color=green>" + dashhotkey + "</color> to dash.\n" +
                                "This can be useful to cross gaps.\n\n You can dash only once while in the air.\n" +
                                "\nOn the ground it can be used for faster movement.";
    }
}
