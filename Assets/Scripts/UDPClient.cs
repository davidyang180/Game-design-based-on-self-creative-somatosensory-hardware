using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPClient : MonoBehaviour
{
    public static string gyro_recvStr;
    public static string control_recvStr;
    private string UDPClientIP;
    string str = "客户端01发送消息";
    Socket gyro_socket;
    EndPoint gyro_serverEnd;
    IPEndPoint gyro_ipEnd;
    Socket control_socket;
    EndPoint control_serverEnd;
    IPEndPoint control_ipEnd;
    static readonly object _locker = new object();
    static readonly object gyro_locker = new object();
    static readonly object control_locker = new object();
    static bool start = false;
    byte[] gyro_recvData = new byte[1024];
    byte[] gyro_sendData = new byte[1024];
    int gyro_recvLen = 0;
    bool gyro_start = false;
    byte[] control_recvData = new byte[1024];
    byte[] control_sendData = new byte[1024];
    int control_recvLen = 0;
    bool control_start = false;
    public static bool move_start = false;
    Thread gyro_connectThread;
    Thread control_connectThread;
    Thread start_connectThread;

    void Start()
    {

    }
    public void Get_IP(string ip_Add)
    {
        UDPClientIP = ip_Add;//服务端的IP.自己更改
        UDPClientIP = UDPClientIP.Trim();
        InitSocket();
    }

    void InitSocket()
    {
        gyro_ipEnd = new IPEndPoint(IPAddress.Parse(UDPClientIP), 3241);
        control_ipEnd = new IPEndPoint(IPAddress.Parse(UDPClientIP), 3242);
        gyro_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        control_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        IPEndPoint sender2 = new IPEndPoint(IPAddress.Any, 0);
        gyro_serverEnd = (EndPoint)sender;
        control_serverEnd = (EndPoint)sender2;
        Debug.Log("等待连接");
        start_connectThread = new Thread(new ThreadStart(connect_start));
        start_connectThread.Start();
        /*
        while (gyro_recvLen == 0)
        {
            SocketSend(str, gyro_ipEnd, gyro_sendData, gyro_socket);
            SocketReceive_gyro();
            Debug.Log("我在这~~~~~");
        }
        Debug.Log("连接a" + "信息来自: " + gyro_serverEnd.ToString() + ',' + gyro_recvStr);
        gyro_start = true;
        gyro_recvLen = 0;
        while (control_recvLen == 0)
        {
            SocketSend(str, control_ipEnd, control_sendData, control_socket);
            SocketReceive_control();
        }
        Debug.Log("连接b" + "信息来自: " + control_serverEnd.ToString() + ',' + control_recvStr);
        control_start = true;
        control_recvLen = 0;
        */
        //开启一个线程连接
        gyro_connectThread = new Thread(new ThreadStart(SocketReceive_gyro));
        gyro_connectThread.Start();
        control_connectThread = new Thread(new ThreadStart(SocketReceive_control));
        control_connectThread.Start();
    }

    public static void start_pi()
    {
        lock (_locker)
        {
            start = true;
            Monitor.Pulse(_locker);//通知等待的队列
        }
    }

    void connect_start()
    {
        lock (_locker)
        {
            while (!start) //只要start字段是false，就等待。
                Monitor.Wait(_locker); //在等待的时候，锁已经被释放了。
        }
        while (gyro_recvLen == 0)
        {
            SocketSend(str, gyro_ipEnd, gyro_sendData, gyro_socket);
            gyro_recvData = new byte[1024];
            gyro_recvLen = gyro_socket.ReceiveFrom(gyro_recvData, ref gyro_serverEnd);
            gyro_recvStr = Encoding.UTF8.GetString(gyro_recvData, 0, gyro_recvLen);
        }
        //Debug.Log("连接a" + "信息来自: " + gyro_serverEnd.ToString() + ',' + gyro_recvStr);
        gyro_recvLen = 0;
        while (control_recvLen == 0)
        {
            SocketSend(str, control_ipEnd, control_sendData, control_socket);
            control_recvData = new byte[1024];
            control_recvLen = control_socket.ReceiveFrom(control_recvData, ref control_serverEnd);
            control_recvStr = Encoding.UTF8.GetString(control_recvData, 0, control_recvLen);
        }
        //Debug.Log("连接b" + "信息来自: " + control_serverEnd.ToString() + ',' + control_recvStr);
        control_recvLen = 0;
        lock (gyro_locker)
        {
            gyro_start = true;
            Monitor.Pulse(gyro_locker);//通知接收陀螺仪位置信息的线程
        }
        lock (control_locker)
        {
            control_start = true;
            Monitor.Pulse(control_locker);//通知接收异步控制信息的线程
        }

    }
    void SocketSend(string sendStr, IPEndPoint ipEnd, byte[] sendData, Socket socket)
    {
        //清空
        sendData = new byte[1024];
        //数据转换
        sendData = Encoding.UTF8.GetBytes(sendStr);
        //发送给指定服务端
        socket.SendTo(sendData, sendData.Length, SocketFlags.None, ipEnd);
    }

    //接收服务器信息
    void SocketReceive_gyro()
    {
        lock (gyro_locker)
        {
            while (!gyro_start) //只要start字段是false，就等待。
                Monitor.Wait(gyro_locker); //在等待的时候，锁已经被释放了。
        }
        move_start = true;
        while (true)
        {

            gyro_recvData = new byte[1024];
            try
            {
                gyro_recvLen = gyro_socket.ReceiveFrom(gyro_recvData, ref gyro_serverEnd);
            }
            catch (Exception e)
            {

            }

            if (gyro_recvLen > 0)
            {
                //Debug.Log("信息来自a: " + gyro_serverEnd.ToString());
                gyro_recvStr = Encoding.UTF8.GetString(gyro_recvData, 0, gyro_recvLen);
                FPSCtrl.getmove(gyro_recvStr);
            }
            else
            {

            }

            //Debug.Log(gyro_recvStr);
        }
    }

    void SocketReceive_control()
    {
        lock (control_locker)
        {
            while (!control_start) //只要start字段是false，就等待。
                Monitor.Wait(control_locker); //在等待的时候，锁已经被释放了。
        }
        while (true)
        {

            control_recvData = new byte[1024];
            try
            {
                control_recvLen = control_socket.ReceiveFrom(control_recvData, ref control_serverEnd);
            }
            catch (Exception e)
            {

            }


            if (control_recvLen > 0 && control_start)
            {
                //Debug.Log("信息来自b: " + control_serverEnd.ToString());
                control_recvStr = Encoding.UTF8.GetString(control_recvData, 0, control_recvLen);
            }
            else
            {

            }

            //Debug.Log(control_recvStr);
        }
    }

    //连接关闭
    void SocketQuit()
    {
        //关闭线程
        if (gyro_connectThread != null)
        {
            gyro_connectThread.Interrupt();
            gyro_connectThread.Abort();
        }
        if (control_connectThread != null)
        {
            control_connectThread.Interrupt();
            control_connectThread.Abort();
        }
        //最后关闭socket
        if (gyro_socket != null)
            gyro_socket.Close();
        if (control_socket != null)
            control_socket.Close();
    }
    void OnApplicationQuit()
    {
        SocketQuit();
    }

    void Update()
    {

    }

}