using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerresetzone : MonoBehaviour
{
    [SerializeField] private Platformonentermove[] moveonenterplatforms;
    [SerializeField] private Platformstate[] switchstateplatforms;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Globalcalls.playeresetpoint != Vector3.zero)
            {
                StartCoroutine(playerreset(collision.gameObject));
            }
        }
    }
    IEnumerator playerreset(GameObject player)
    {
        player.GetComponent<Playerstatemachine>().move = Vector2.zero;
        player.GetComponent<Playerstatemachine>().resetplayer();
        player.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        player.SetActive(true);
        player.transform.position = Globalcalls.playeresetpoint;
        player.GetComponent<Playerstatemachine>().resetplayer();
        if (moveonenterplatforms.Length != 0)
        {
            for (int i = 0; i < moveonenterplatforms.Length; i++)
            {
                moveonenterplatforms[i].resetforminstant();
            }
        }
        if(switchstateplatforms.Length != 0)
        {
            for (int i = 0; i < switchstateplatforms.Length; i++)
            {
                switchstateplatforms[i].resetswitchplatform();
            }
        }
    }
}
