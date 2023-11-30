using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movingbackground : MonoBehaviour
{
    private Transform cam;
    private Vector2 camstartposi;
    private float distance;

    private GameObject[] backgrounds;
    private Material[] mats;
    private float[] backspeed;

    private float farthestback;
    [Range(0.1f, 6f)] public float backgroundspeed;

    private bool dontupdateafterreset;

    private void Awake()
    {
        cam = Camera.main.transform;
        camstartposi = cam.position;

        int backgroundcount = transform.childCount;
        backgrounds = new GameObject[backgroundcount];
        mats = new Material[backgroundcount];
        backspeed = new float[backgroundcount];

        for (int i = 0; i < backgroundcount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            mats[i] = backgrounds[i].GetComponent<Renderer>().material;
        }
        backspeedcalculation(backgroundcount);
    }
    private void backspeedcalculation(int backgroundcount)
    {
        for (int i = 0; i < backgroundcount; i++)
        {
            if((backgrounds[i].transform.position.z - cam.position.z) > farthestback)
            {
                farthestback = backgrounds[i].transform.position.z - cam.position.z;
            }
        }
        for (int i = 0; i < backgroundcount; i++)
        {
            backspeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / farthestback;
        }
    }
    private void LateUpdate()
    {
        distance = cam.position.x - camstartposi.x;
        transform.position = new Vector3(cam.position.x, cam.position.y, 0);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float speed = backspeed[i] * backgroundspeed * 0.1f;
            mats[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
        }
    }
}
