using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public float MaxHP;
    public float HP;
    public GameObject MainCam;
    public GameObject Canv;
    public string Name;
    public Slider Sld;
    public Text NameTxt;
    public List<EnemyWeapon> Weapons;
    public GameObject DieExplodeObj;
    public OneEnemy Ctrler;
    // Start is called before the first frame update
    void Start()
    {
        Canv.GetComponent<Canvas>().worldCamera = MainCam.GetComponent<Camera>();
        Sld.maxValue = MaxHP;
        Sld.value = HP;
        NameTxt.text = Name;
        OnPlay.Enemies.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Canv.transform.LookAt(MainCam.transform);
        Sld.value = HP;
    }

    public void OnHit(float atk)
    {
        //击中敌舰后
        HP -= atk;
        if (HP <= 0)
        {
            OnPlay.s += 4;
            HP = 0;
            GameObject obj =  Instantiate(DieExplodeObj, transform);
            obj.transform.parent = null;
            Ctrler.OnDie();
        }
    }

    private void OnDestroy()
    {
        OnPlay.Enemies.Remove(gameObject);
    }
}
