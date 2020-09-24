using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float Lifetime = 1;
    private float DestroyTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        DestroyTime = Time.time + Lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > DestroyTime)
        {
            Destroy(gameObject);
        }
    }
}
