using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connection : MonoBehaviour
{
    public UDPClient UDP_Cli;
    public Button connectButt;
    public Text IP_Add;
    // Start is called before the first frame update
    void Start()
    {
        connectButt.onClick.AddListener(conF);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void conF()
    {
        string add = IP_Add.text;
        UDP_Cli.Get_IP(add);
        UDPClient.start_pi();
    }
}
