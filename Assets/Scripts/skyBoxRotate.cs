using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyBoxRotate : MonoBehaviour
{
    float num;

    // Update is called once per frame
    void Update()
    {
        num = RenderSettings.skybox.GetFloat("_Rotation");
        RenderSettings.skybox.SetFloat("_Rotation", num + 0.05f);
    }
}