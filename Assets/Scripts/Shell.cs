using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public GameObject ExplosionObj;
    public float ShellSpeed = 10;
    public float Lifetime = 3;
    private float DestroyTime = 0;
    public float Damage0 = 2;
    public float Damage1 = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > DestroyTime)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(Transform Target)
    {

        transform.parent = null;
        transform.LookAt(Target);
        GetComponent<Rigidbody>().velocity = transform.forward * ShellSpeed;
        DestroyTime = Time.time + Lifetime;
        Damage0 *= OnPlay.Damage0_mult;
        Damage1 *= OnPlay.Damage1_mult;
    }

    private void FixedUpdate()
    {
        Ray CheckHit = new Ray(transform.position, transform.forward);
        RaycastHit hit;//定义射线碰撞
        if (Physics.Raycast(CheckHit, out hit))
        {
            if(hit.distance <= ShellSpeed * Time.fixedDeltaTime)
            {
                //print("命中" + hit.collider.name);
                if(hit.collider.gameObject.GetComponent<Enemy>()!=null)
                    hit.collider.gameObject.GetComponent<Enemy>().OnHit(Damage0);
                if(hit.collider.gameObject.GetComponent<SelfOnHit>() != null)
                    hit.collider.gameObject.GetComponent<SelfOnHit>().OnHit(Damage1);
                Explode(hit.point, hit.normal);
            }
        }

    }
    void Explode(Vector3 loc,Vector3 dir)
    {
        GameObject obj = Instantiate(ExplosionObj, loc,Quaternion.LookRotation(dir));
        Destroy(gameObject);
    }
}
