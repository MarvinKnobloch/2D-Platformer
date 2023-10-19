using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movementreset : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out Playerstatemachine playerstatemachine))
            {
                if (playerstatemachine.currentdashcount > 0) playerstatemachine.currentdashcount--;
                playerstatemachine.doublejump = true;
            }
            gameObject.SetActive(false);
        }
    }
}
