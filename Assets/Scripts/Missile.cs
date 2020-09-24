using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject ExplosionObj;
    public float ShellSpeed = 10;
    public float Lifetime = 5;
    private float DestroyTime = 0;
    public float Damage0 = 50;
    public float Damage1 = 5;
    Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        Damage1 *= OnPlay.Damage1_mult;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > DestroyTime)
        {
            Destroy(gameObject);
        }
    }
    public void Setup(Transform target)
    {
        Target = target;
        transform.parent = null;
        GetComponent<Rigidbody>().velocity = transform.forward * ShellSpeed;
        DestroyTime = Time.time + Lifetime;
    }

    private void FixedUpdate()
    {
        if(Target!=null)
            transform.LookAt(Target);
        GetComponent<Rigidbody>().velocity = transform.forward * ShellSpeed;
        Ray CheckHit = new Ray(transform.position, transform.forward);
        RaycastHit hit;//定义射线碰撞
        if (Physics.Raycast(CheckHit, out hit))
        {
            if (hit.distance <= ShellSpeed * Time.fixedDeltaTime)
            {
                //print("命中" + hit.collider.name);
                if (hit.collider.gameObject.GetComponent<Enemy>() != null)
                    hit.collider.gameObject.GetComponent<Enemy>().OnHit(Damage0);
                if (hit.collider.gameObject.GetComponent<SelfOnHit>() != null)
                    hit.collider.gameObject.GetComponent<SelfOnHit>().OnHit(Damage1);
                //Debug.Log(Damage1);
                Explode(hit.point, hit.normal);
            }
        }
    }
    void Explode(Vector3 loc, Vector3 dir)
    {
        GameObject obj = Instantiate(ExplosionObj, loc, Quaternion.LookRotation(dir));
        Destroy(gameObject);
    }
}
