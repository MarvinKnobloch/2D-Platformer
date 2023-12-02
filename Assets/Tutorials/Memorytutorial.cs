using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Memorytutorial : MonoBehaviour
{
    private TextMeshProUGUI tutorialtext;
    private Controlls controls;

    private string memoryhotkey;

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
        //memoryhotkey = controls.Player.Memorie.GetBindingDisplayString();
        //tutorialtext.text = "After stepping on the \"M\" button press <color=green>" + memoryhotkey + "</color> to place a memory of your character.\n" +
        //                    "This memory will remain a few seconds or until you use this ability again which will teleport you back to the position of the memory.\n" + 
        //                    "The number on the bottom left will display how often you can use this ability.";

        memoryhotkey = controls.Player.Memorie.GetBindingDisplayString();
        tutorialtext.text = "Step into the yellow light to gain the memory ability.\n" +
                            "\nPress <color=green>" + memoryhotkey + "</color> to use it which will create a memory of your character.\n" +
                            "This memory will remain a few seconds or until you use this ability again which will teleport you back to the position of the memory.\n" +
                            "\nThe icon on the bottom left will display how often you can use this ability.";
    }
}
