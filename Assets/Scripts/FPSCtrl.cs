using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCtrl : MonoBehaviour
{
    public Camera MainCam;
    public Transform Turret;
    public Transform GunSeat;
    public Transform Turret2;
    public Transform GunSeat2;
    public Transform MissileSeat;
    public float VSpeed;
    public float HSpeed;
    public List<Transform> Guns;
    public List<Transform> Guns2;
    private int now_fire = 0;
    private int now_fire2 = 0;
    public GameObject ShellObj;
    public GameObject MissileObj;
    public float cooltime = 0.2f;
    private float LastFireTime = 0;
    public float MissileLocateRange = 30;
    public float MissileLocateAngle = 0.02f;
    public AudioSource MachineAS;
    public AudioSource MissileLockAS;
    bool isMachineRotate = false;
    bool MachineInertia = false;
    bool isMissileLocating = false;
    public Transform TargetPoint;
    GameObject LocatingTarget;
    public UnityEngine.UI.Image LocatingAim;
    public UnityEngine.UI.Image LocatingRect;
    public UnityEngine.UI.Image NoMissile;
    public UnityEngine.UI.Text LocatingRectTxt;
    public Color NormalColor;
    public Color AlertColor;
    public Color CautionColor;
    public int MissileMaxClip = 5;
    public int MissileCount = 10;
    public int MissileClip = 5;
    public UnityEngine.UI.Text Score;
    public UnityEngine.UI.Text MissileClipTxt;

    public static float Pitch;
    public static float Yaw;
    public static float Roll;
    public static string[] sArray;
    public float speed = 1f;

    public static void getmove(string client_gyrodata)
    {
        sArray = client_gyrodata.Split(new char[1] { ',' });
        float.TryParse(sArray[0], out Pitch);
        float.TryParse(sArray[1], out Yaw);
        float.TryParse(sArray[2], out Roll);
        /*
        y = (640 - y) / 53 - 6;
        x = -x / 120 + 2;
        big = -12 + (Mathf.Sqrt(big * big / 5) / 2000);
        */
        //Debug.Log("这是getmove"+ Pitch + ',' + Yaw + ',' + Roll);

    }
    // Start is called before the first frame update
    void Start()
    {
        MissileClipTxt.text = MissileMaxClip.ToString() + "/" + MissileCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Score.text = OnPlay.s.ToString();
        if (isMissileLocating)
        {
            //导弹自动瞄准
            MissileSearch();
        }
        if (UDPClient.control_recvStr == "110")//--------------------------------------------------抬起鼠标右键
        {
            if (MissileClip > 0)
            {
                MissileClip -= 1;
                MissileClipTxt.text = MissileClip.ToString() + "/" + MissileCount.ToString();
                MissileLockAS.pitch = 0.1f;
                MissileLockAS.Stop();
                isMissileLocating = false;
                if (LocatingTarget != null)
                {
                    FireMissile(LocatingTarget.transform);
                }
                LocatingTarget = null;
                LocatingRect.gameObject.SetActive(false);
                Debug.Log("发射导弹");
                //print("隐藏GUI");
                UDPClient.control_recvStr = "011";
            }
        }
        else if (UDPClient.control_recvStr == "101")//--------------------------------------------------按下鼠标右键
        {
            if (MissileClip > 0)
            {
                MissileLockAS.Play();
                isMissileLocating = true;
                LocatingRect.color = NormalColor;
                LocatingRectTxt.text = "Searching";
                LocatingRectTxt.color = NormalColor;
                LocatingRect.gameObject.SetActive(true);
                Debug.Log("瞄准");
            }
            else
            {
                NoMissile.gameObject.SetActive(true);
                Debug.Log("瞄准");
            }
        }
    }

    private void FixedUpdate()
    {
        isMachineRotate = false;
        
        if (UDPClient.move_start) {
            isMachineRotate = true;
            float step = speed * Time.deltaTime;
            //GunSeat.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, new Vector3(y, x, big), step);
            GunSeat.localEulerAngles = new Vector3(Pitch, Yaw, 0);
            if (GunSeat.localEulerAngles.x < 320 && GunSeat.localEulerAngles.x > 180)
                GunSeat.localEulerAngles = new Vector3(320, GunSeat.localEulerAngles.y, GunSeat.localEulerAngles.z);
            if (GunSeat.localEulerAngles.x > 0 && GunSeat.localEulerAngles.x < 180)
                GunSeat.localEulerAngles = new Vector3(0, GunSeat.localEulerAngles.y, GunSeat.localEulerAngles.z);
            if (Turret.localEulerAngles.y < 300 && Turret.localEulerAngles.y > 180)
                Turret.localEulerAngles = new Vector3(Turret.localEulerAngles.x, 300, Turret.localEulerAngles.z);
            if (Turret.localEulerAngles.y > 60 && Turret.localEulerAngles.y < 180)
                Turret.localEulerAngles = new Vector3(Turret.localEulerAngles.x, 60, Turret.localEulerAngles.z);
            //Debug.Log("欧拉角为"+GunSeat.localEulerAngles);
        }
        
        
        /*
        if (Input.GetKey(KeyCode.W))//--------------------------------------------------按W
        {
            isMachineRotate = true;
            GunSeat.localEulerAngles = new Vector3(GunSeat.localEulerAngles.x - VSpeed * Time.fixedDeltaTime, GunSeat.localEulerAngles.y, GunSeat.localEulerAngles.z);
            if (GunSeat.localEulerAngles.x < 320 && GunSeat.localEulerAngles.x > 180)
                GunSeat.localEulerAngles = new Vector3(320, GunSeat.localEulerAngles.y, GunSeat.localEulerAngles.z);
        }
        else if (Input.GetKey(KeyCode.S))//--------------------------------------------------按S
        {
            isMachineRotate = true;
            GunSeat.localEulerAngles = new Vector3(GunSeat.localEulerAngles.x + VSpeed * Time.fixedDeltaTime, GunSeat.localEulerAngles.y, GunSeat.localEulerAngles.z);
            if (GunSeat.localEulerAngles.x > 0 && GunSeat.localEulerAngles.x < 180)
                GunSeat.localEulerAngles = new Vector3(0, GunSeat.localEulerAngles.y, GunSeat.localEulerAngles.z);
        }
        if (Input.GetKey(KeyCode.A))//--------------------------------------------------按A
        {
            isMachineRotate = true;
            Turret.localEulerAngles = new Vector3(Turret.localEulerAngles.x, Turret.localEulerAngles.y - HSpeed * Time.fixedDeltaTime, Turret.localEulerAngles.z);
            if (Turret.localEulerAngles.y < 300 && Turret.localEulerAngles.y > 180)
                Turret.localEulerAngles = new Vector3(Turret.localEulerAngles.x,300, Turret.localEulerAngles.z);

        }
        else if (Input.GetKey(KeyCode.D))//--------------------------------------------------按D
        {
            isMachineRotate = true;
            Turret.localEulerAngles = new Vector3(Turret.localEulerAngles.x, Turret.localEulerAngles.y + HSpeed * Time.fixedDeltaTime, Turret.localEulerAngles.z);
            if (Turret.localEulerAngles.y > 60 && Turret.localEulerAngles.y < 180)
                Turret.localEulerAngles = new Vector3(Turret.localEulerAngles.x, 60, Turret.localEulerAngles.z);
        }
        */
        //Input.GetKey(KeyCode.R)
        if (UDPClient.control_recvStr == "100")//--------------------------------------------------按R换弹
        {
            Debug.Log("换弹");
            if ((MissileMaxClip - MissileClip) <= MissileCount)
            {
                MissileCount = MissileCount - MissileMaxClip + MissileClip;
                MissileClip = MissileMaxClip;
                NoMissile.gameObject.SetActive(false);
                Debug.Log("1");
            }
            else
            {
                MissileClip = MissileCount + MissileClip;
                MissileCount = 0;
                NoMissile.gameObject.SetActive(false);
                Debug.Log("2");
            }
            MissileClipTxt.text = MissileClip.ToString() + "/" + MissileCount.ToString();
            UDPClient.control_recvStr = "011";
        }
        if (isMachineRotate)
        {
            GunSeat2.localEulerAngles = GunSeat.localEulerAngles;
            Turret2.localEulerAngles = Turret.localEulerAngles;
            MissileSeat.LookAt(TargetPoint);
        }
        //Input.GetKey(KeyCode.Mouse0)
        if (UDPClient.control_recvStr == "001")//--------------------------------------------------按下鼠标左键
        {
            Debug.Log("射击");
            if (Time.time > LastFireTime + cooltime)
            {
                Fire();
                //Fire2();
                LastFireTime = Time.time;
            }
        }
        if (MachineAS.isPlaying)
        {
            if (!isMachineRotate && !MachineInertia)
            {
                MachineAS.time = 2.5f;
                MachineInertia = true;
            }
            else if (isMachineRotate && MachineInertia)
            {
                MachineAS.time = 0.0f;
                MachineInertia = false;
            }
        }
        else if (isMachineRotate)
        {
            MachineAS.time = 0.0f;
            MachineInertia = false;
            MachineAS.Play();
        }
    }
    private void Fire()
    {
        GameObject gunshell = Instantiate(ShellObj, Guns[now_fire].transform);
        gunshell.GetComponent<Shell>().Setup(TargetPoint);
        now_fire++;
        if (now_fire >= Guns.Count)
        {
            now_fire = 0;
        }
    }
    private void Fire2()
    {
        GameObject gunshell = Instantiate(ShellObj, Guns2[now_fire2].transform);
        gunshell.GetComponent<Shell>().Setup(TargetPoint);
        now_fire2++;
        if (now_fire2 >= Guns2.Count)
        {
            now_fire2 = 0;
        }
    }
    private void FireMissile(Transform Target)
    {
        GameObject missile = Instantiate(MissileObj, MissileSeat.position,Quaternion.LookRotation(MissileSeat.forward));
        missile.GetComponent<Missile>().Setup(Target);
    }
    void MissileSearch()
    {
        Vector3 temp;
        float nowDis = 0;
        bool isCatch = false;
        if (LocatingTarget == null)
        {
            float minDis = 0;
            foreach (GameObject en in OnPlay.Enemies)
            {
                if ((en.transform.position - Turret.position).magnitude <= MissileLocateRange)
                {
                    temp = MainCam.WorldToViewportPoint(en.transform.position) - MainCam.WorldToViewportPoint(TargetPoint.position);
                    nowDis = temp.x * temp.x + temp.y * temp.y;
                    if (nowDis <= MissileLocateAngle)
                    {
                        if (isCatch)
                        {
                            if (nowDis < minDis)
                            {
                                minDis = nowDis;
                                LocatingTarget = en;
                            }
                        }
                        else
                        {
                            minDis = nowDis;
                            isCatch = true;
                            LocatingTarget = en;
                        }
                    }
                }
            }
            if (isCatch)
            {
                LocatingRect.color = AlertColor;
                LocatingRectTxt.text = "Locked";
                LocatingRectTxt.color = AlertColor;
            }
        }
        else
        {
            if ((LocatingTarget.transform.position - Turret.position).magnitude <= MissileLocateRange)
            {
                temp = MainCam.WorldToViewportPoint(LocatingTarget.transform.position) - MainCam.WorldToViewportPoint(TargetPoint.position);
                nowDis = temp.x * temp.x + temp.y * temp.y;
                if (nowDis <= MissileLocateAngle)
                {
                    MissileLockAS.pitch = 0.7f;
                    isCatch = true;
                }
            }
            if (!isCatch)
            {
                MissileLockAS.pitch = 0.1f;
                LocatingTarget = null;
                LocatingRect.color = NormalColor;
                LocatingRectTxt.text = "Searching";
                LocatingRectTxt.color = NormalColor;
            }
        }
    }
}
