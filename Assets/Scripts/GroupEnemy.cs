using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupEnemy : MonoBehaviour
{
    public List<GameObject> Members;
    public GameObject Cam;
    public GameObject Target;
    public List<GameObject> EnemyTypes;
    public float TeamSpeed;

    public void setup(GameObject _Cam, GameObject _Tar)
    {
        for (int i = 0; i < Members.Count; i++)
        {
            Members[i].GetComponent<OneEnemy>().Speed = TeamSpeed;
            Members[i].GetComponent<OneEnemy>().EnemyType = EnemyTypes[i];
            Members[i].GetComponent<OneEnemy>().setup(_Cam, _Tar);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool isDestroyed = true;
        foreach (GameObject go in Members)
        {
            if (go != null)
                isDestroyed = false;
        }
        if (isDestroyed)
            Destroy(gameObject);
    }
}
