using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Memorytimer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Playerstatemachine playerstatemachine;
    private float memorytime;
    private Image cdimage;
    private void OnEnable()
    {
        playerstatemachine = player.GetComponent<Playerstatemachine>();
        cdimage = GetComponent<Image>();
        memorytime = playerstatemachine.memorymaxusetime;
        cdimage.fillAmount = 1;
        StartCoroutine("disablememorycd");
    }
    private IEnumerator disablememorycd()
    {
        while (memorytime > 0)
        {
            memorytime -= Time.deltaTime;
            cdimage.fillAmount = memorytime / playerstatemachine.memorymaxusetime;
            transform.parent.position = player.transform.position + Vector3.up * 0.8f;
            yield return null;
        }
        StopCoroutine("disablememorycd");
        playerstatemachine.endmemorytimer();
        transform.parent.gameObject.SetActive(false);
    }
    public void disablecd()
    {
        StopCoroutine("disablememorycd");
        playerstatemachine.endmemorytimer();
        transform.parent.gameObject.SetActive(false);
    }
}
