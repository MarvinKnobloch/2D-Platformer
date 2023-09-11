using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Platformstate : MonoBehaviour
{
    public bool isactiv;
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
        if (isactiv == true)
        {
            spriteRenderer.color = Color.white;
        }
        else spriteRenderer.color = inactivcolor;
    }
    public void switchplatform()
    {
        if (isactiv == true)
        {
            isactiv = false;
            spriteRenderer.color = inactivcolor;
            foreach (BoxCollider2D cols in boxcolliders)
            {
                cols.enabled = false;
            }
        }
        else
        {
            isactiv = true;
            spriteRenderer.color = Color.white;
            foreach (BoxCollider2D cols in boxcolliders)
            {
                cols.enabled = true;
            }
        }
    }
    public void switchtored()
    {
        if (isactiv == true)
        {
            isactiv = false;
            spriteRenderer.color = inactivcolor;
            foreach (BoxCollider2D cols in boxcolliders)
            {
                cols.enabled = false;
            }
        }
    }
}
