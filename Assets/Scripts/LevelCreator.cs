using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> Location;
    public GameObject GroupEnemy1;
    public GameObject GroupEnemy2;
    public GameObject GroupEnemy3;
    public GameObject GroupEnemy4;
    public GameObject MainCam;
    public GameObject target;
    public float time;
    public int i = 0;
    void Start()
    {
        time = 600;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
        time ++;
        if (time == 800)
        {
            if(i == 0)
            {
                CreatEnemy(1, 1);
                CreatEnemy(3, 1);
                CreatEnemy(2, 1);
                CreatEnemy(2, 2);
            }
            else if (i == 1)
            {
                CreatEnemy(2, 2);
                CreatEnemy(1, 1);
                CreatEnemy(3, 1);
                CreatEnemy(2, 2);
            }
            else if (i == 2)
            {
                CreatEnemy(3, 1);
                CreatEnemy(2, 1);
                CreatEnemy(4, 2);
                CreatEnemy(2, 2);
            }
            else if (i == 3)
            {
                CreatEnemy(1, 2);
                CreatEnemy(2, 2);
                CreatEnemy(4, 1);
                CreatEnemy(3, 2);
            }
            else if (i == 4)
            {

                CreatEnemy(1, 3);
                CreatEnemy(2, 1);
                CreatEnemy(2, 2);
            }
            i += 1;
            time = 0;
            //Debug.Log("zero");
        }
        if (time == 700)
        {

            if (i == 0)
            {
                CreatEnemy(2, 1);
            }
            else if (i == 1)
            {
                CreatEnemy(3, 1);
            }
            else if (i == 2)
            {
                CreatEnemy(1, 2);
            }
            else if (i == 3)
            {
                CreatEnemy(3, 3);
            }
            else if (i == 4)
            {
                CreatEnemy(2, 4);
            }
        }
        //Debug.Log(time);
        
    }

    void CreatEnemy(int loc,int enenum)
    {
        GameObject Enemy;
        switch (enenum)
        {
            case 1:
                Enemy = Instantiate(GroupEnemy1, Location[loc-1].transform);
                Enemy.GetComponent<GroupEnemy>().setup(MainCam, target);
                break;
            case 2:
                Enemy = Instantiate(GroupEnemy2, Location[loc-1].transform);
                Enemy.GetComponent<GroupEnemy>().setup(MainCam, target);
                break;
            case 3:
                Enemy = Instantiate(GroupEnemy3, Location[loc-1].transform);
                Enemy.GetComponent<GroupEnemy>().setup(MainCam, target);
                break;
            case 4:
                Enemy = Instantiate(GroupEnemy4, Location[loc-1].transform);
                Enemy.GetComponent<GroupEnemy>().setup(MainCam, target);
                break;
        }
    }
}
