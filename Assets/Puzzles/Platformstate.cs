using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Platformstate : MonoBehaviour
{
    [SerializeField] private bool isactivonstart;
    private bool isactiv;
    private SpriteRenderer spriteRenderer;
    private Color inactivcolor;
    private BoxCollider2D[] boxcolliders;

    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#840000", out inactivcolor);
        inactivcolor.a = 0.3f;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        boxcolliders = GetComponentsInChildren<BoxCollider2D>();
    }
    private void OnEnable()
    {
        if (isactivonstart) switchtoactive();
        else switchtonotactive();

    }
    public void switchplatform()
    {
        if (isactiv == true) switchtonotactive();
        else switchtoactive();
    }
    public void switchtored()
    {
        if (isactiv == true) switchtonotactive();
    }
    private void switchtoactive()
    {
        isactiv = true;
        spriteRenderer.color = Color.white;
        foreach (BoxCollider2D cols in boxcolliders)
        {
            cols.enabled = true;
        }
    }
    private void switchtonotactive()
    {
        isactiv = false;
        spriteRenderer.color = inactivcolor;
        foreach (BoxCollider2D cols in boxcolliders)
        {
            cols.enabled = false;
        }
    }
    public void resetswitchplatform()
    {
        if (isactivonstart == true) switchtoactive();
        else switchtonotactive();
    }
}
