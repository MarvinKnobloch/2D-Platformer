using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Platformstate : MonoBehaviour
{
    [SerializeField] private bool isactivonstart;
    private bool isactiv;
    private SpriteRenderer spriteRenderer;
    private Color activecolor;
    private Color inactivecolor;
    private BoxCollider2D[] boxcolliders;

    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#FFFFFF", out activecolor);
        activecolor.a = 1;
        ColorUtility.TryParseHtmlString("#FFFFFF", out inactivecolor);
        inactivecolor.a = 0.35f;
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
        spriteRenderer.color = activecolor;
        foreach (BoxCollider2D cols in boxcolliders)
        {
            cols.enabled = true;
        }
    }
    private void switchtonotactive()
    {
        isactiv = false;
        spriteRenderer.color = inactivecolor;
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
