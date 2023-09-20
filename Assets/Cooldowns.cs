using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cooldowns : MonoBehaviour
{
    public static Cooldowns instance;

    [SerializeField] private GameObject gravitystacksui;
    [SerializeField] private TextMeshProUGUI gravitystacks;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void displaygravitystacks()
    {
        gravitystacksui.SetActive(true);
        gravitystacks.text = Globalcalls.currentgravitystacks.ToString();
    }
    public void handlegravitystacks()
    {
        if (Globalcalls.currentgravitystacks == 0) gravitystacksui.SetActive(false);
        else gravitystacks.text = Globalcalls.currentgravitystacks.ToString();
    }
}
