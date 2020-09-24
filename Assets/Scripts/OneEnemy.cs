using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneEnemy : MonoBehaviour
{
    public GameObject Target;
    public float Speed;
    public float StartAttackRange = 25;
    public float AttackAdjustRange = 20;
    public float FinishAttackRange = 15;
    public GameObject EnemyType;
    GameObject EnemySelf;
    bool isCreated = false;
    enum State
    {
        Arriving,
        Attacking,
        LeaveTurning,
        Leaving,
        ExitCruising
    }
    State state;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setup(GameObject _Camera, GameObject _Target)
    {
        EnemySelf = Instantiate(EnemyType, transform);
        EnemySelf.GetComponent<Enemy>().MainCam = _Camera;
        Target = _Target;
        EnemySelf.GetComponent<Enemy>().Ctrler = this;
        state = State.Arriving;
        transform.LookAt(Target.transform);
        GetComponent<Rigidbody>().velocity = transform.forward * Speed;
        isCreated = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    Vector3 AttackAngle;
    Vector3 LeaveTurningStartToward;
    float fmod;
    private void FixedUpdate()
    {
        if (!isCreated)
            return;
        switch (state)
        {
            case State.Arriving:
                transform.LookAt(Target.transform);
                if ((transform.position - Target.transform.position).magnitude <= StartAttackRange)
                {
                    state = State.Attacking;
                    AttackAngle = transform.localEulerAngles;
                    foreach (EnemyWeapon wp in EnemySelf.GetComponent<Enemy>().Weapons)
                    {
                        wp.target = Target.transform;
                        wp.SetStartFire();
                    }
                    
                }
                break;
            case State.Attacking:
                if ((transform.position - Target.transform.position).magnitude <= AttackAdjustRange)
                {
                    transform.localEulerAngles = new Vector3(AttackAngle.x, AttackAngle.y, AttackAngle.z - 50.0f * (1 -
                        ((transform.position - Target.transform.position).magnitude - FinishAttackRange) / (AttackAdjustRange - FinishAttackRange)));
                }
                if ((transform.position - Target.transform.position).magnitude <= FinishAttackRange)
                {
                    GetComponent<Rigidbody>().angularVelocity = - transform.right * Speed * 0.3f;
                    LeaveTurningStartToward = Target.transform.position - transform.position;
                    for (int i = EnemySelf.GetComponent<Enemy>().Weapons.Count - 1; i >= 0; i--)
                    {
                        EnemySelf.GetComponent<Enemy>().Weapons[i].SetFinishFire();
                    }
                    state = State.LeaveTurning;
                }
                break;
            case State.LeaveTurning:
                if (Vector3.Dot(LeaveTurningStartToward.normalized, transform.forward.normalized) < -0.7f)
                {
                    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    fmod = transform.eulerAngles.z;
                    while (fmod > 180.0f)
                        fmod -= 360.0f;
                    while (fmod < -180.0f)
                        fmod += 360.0f;
                    state = State.Leaving;
                }
                break;
            case State.Leaving:
                if ( fmod >= -Speed * 0.5f && fmod < Speed * 0.5f)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y);
                    state = State.ExitCruising;
                }
                else
                {
                    if (fmod > 0)
                    {
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, fmod -= Speed * 0.3f);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, fmod += Speed * 0.3f);
                    }
                }
                break;
            case State.ExitCruising:
                if ((transform.position - Target.transform.position).magnitude >= Speed * 10)
                {
                    Destroy(gameObject);
                    return;
                }
                break;
            default:
                break;
        }
        GetComponent<Rigidbody>().velocity = transform.forward * Speed;
    }

    public void OnDie()
    {
        Destroy(gameObject);
    }
}
