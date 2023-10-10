using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisioncontroller : MonoBehaviour
{
    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(7, 8);
    }
}
