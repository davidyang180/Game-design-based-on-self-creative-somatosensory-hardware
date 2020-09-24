using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfOnHit : MonoBehaviour
{
    public float MaxHP = 100;
    public float HP;
    public Slider HPSlider;
    public GameObject GameOver;
    public Text FinScore;
    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
        HPSlider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnHit(float atk)
    {
        //被击中后
        HP -= atk;
        if (HP <= 0)
        {
            HP = 0;
            Time.timeScale = 0;
            FinScore.text = "Your Scores : " + OnPlay.s.ToString();
            GameOver.SetActive(true);
        }
        HPSlider.value = HP / MaxHP;
        //Debug.Log(HP);
    }
}
