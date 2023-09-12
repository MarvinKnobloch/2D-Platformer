using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotateplatform : MonoBehaviour
{
    private void OnEnable()
    {
        Playergravityswitch.platformrotate += rotateplatform;
    }
    private void OnDisable()
    {
        Playergravityswitch.platformrotate -= rotateplatform;
    }
    private void rotateplatform()
    {
        transform.Rotate(180, 0, 0, Space.World);
    }
}
