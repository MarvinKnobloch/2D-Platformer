using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Cooldowns : MonoBehaviour
{
    public static Cooldowns instance;

    [SerializeField] private GameObject gravitystacksui;
    [SerializeField] private TextMeshProUGUI gravitystacks;
    private Color gravityuicolor;

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
        Globalcalls.currentgravitystacks = 0;
        StartCoroutine("disableonawake");
    }
    
    IEnumerator disableonawake()                    //damit bild und text geladen werden, sonst ensteht ein spike wenn das image das erste mal aktiviert wird
    {
        yield return new WaitForSeconds(0.03f);
        gravityuicolor = gravitystacksui.GetComponent<Image>().color;
        gravityuicolor.a = 1;
        gravitystacksui.GetComponent<Image>().color = gravityuicolor;
        if (Globalcalls.currentgravitystacks == 0) gravitystacksui.SetActive(false);

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
