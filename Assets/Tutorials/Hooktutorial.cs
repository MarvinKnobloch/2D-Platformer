using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Hooktutorial : MonoBehaviour
{
    private TextMeshProUGUI tutorialtext;
    private Controlls controls;

    private string wipehotkey;

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
        wipehotkey = controls.Player.Whip.GetBindingDisplayString();
        tutorialtext.text = "Press <color=green>" + wipehotkey + "</color> to use the whip.\n" +
                            "If your character is close enough to a whip point it will turn green and can be used. The green point also determine which is your current whip target.\n" +
                            "Being below a wipe point will pull your character more upwards";

        wipehotkey = controls.Player.Whip.GetBindingDisplayString();
        tutorialtext.text = "If your character is close enough to a whip point it will turn green and can be used by pressing <color=green>" + wipehotkey + "</color>.\n" +
                            //"Press <color=green>" + wipehotkey + "</color> to activate the whip.\n" +
                            "\nThe green point also determine which is your current whip target.\n" +
                            "\nBeing straight below a wipe point will pull your character more upwards";
    }
}
