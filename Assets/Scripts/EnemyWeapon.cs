using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public enum WeaponType
    {
        Laser,
        Gun,
        Missile
    }
    public WeaponType type;
    public float CoolTime = 0.2f;
    public GameObject ProjectileObj;
    private float LastFireTime = 0;
    private bool isOnFire = false;
    public bool isThrowAwayMissile = false;
    public Transform target;
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnFire && type == WeaponType.Gun)
        {
            if (Time.time > LastFireTime + CoolTime)
            {
                Fire();
                LastFireTime = Time.time;
            }
        }

    }

    public void SetStartFire()
    {
        if (type == WeaponType.Gun || type == WeaponType.Laser)
        {
            isOnFire = true;
        }
        else if (type == WeaponType.Missile)
        {
            //锁定警告什么的
        }
    }

    public void SetFinishFire()
    {
        if (type == WeaponType.Gun || type == WeaponType.Laser)
        {
            isOnFire = false;
        }
        else if (type == WeaponType.Missile)
        {
            Fire();
        }
    }

    void Fire()
    {
        if (type == WeaponType.Gun)
        {
            GameObject gunshell = Instantiate(ProjectileObj, transform);
            gunshell.GetComponent<Shell>().Setup(target);
        }
        else if (type == WeaponType.Missile)
        {

            GameObject missile = Instantiate(ProjectileObj, transform);
            missile.GetComponent<Missile>().Setup(target);
            missile.transform.parent = null;
            if (isThrowAwayMissile)
            {
                parent.GetComponent<Enemy>().Weapons.Remove(this);
                Destroy(gameObject);
            }
        }
    }
}
