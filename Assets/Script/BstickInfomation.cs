using System.Collections.Generic;
using Bstick;
using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine;
using static Bstick.Common;

public class BstickInfomation : MonoBehaviour, IBstickInterface
{
    public BstickDirection Direction;
    private FingerBending _bending;

    void Awake()
    {
        _bending = GetComponent<FingerBending>();
    }
    void Start()
    {
        BstickBridge.Instance.CreateDevice(Direction, this);
    }
    public void CreateDevice(bool success)
    {
        BstickBridge.Instance.ConnectDevice(Direction);
    }
    public bool Connected(bool connect)
    {
        Debug.Log(string.Format("{0} is Connect {1}", Direction, connect));
        //throw new System.NotImplementedException();
        return true;
    }
    public void Disconnected(bool disconnect)
    {
        //throw new System.NotImplementedException();
    }
    public void SendToPosition(List<int> data)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => _bending.BendFingers(data));
    }
    public void SendToPressure(List<int> data)
    {
        //throw new System.NotImplementedException();
    }
    public void SendToTouch(List<int> data)
    {
        //throw new System.NotImplementedException();
    }
    public void SendToQuaternion(List<float> data)
    {
        //throw new System.NotImplementedException();
    }
    public void SendToAccelerometer(List<float> data)
    {
        //throw new System.NotImplementedException();
    }
    public void SendToEuler(List<float> data)
    {
        //throw new System.NotImplementedException();
    }
    public void SendToGyroscope(List<float> data)
    {
        //throw new System.NotImplementedException();
    }
    public void SendToCompass(List<float> data)
    {
        //throw new System.NotImplementedException();
    }
    public void SendCommand(byte[] cmd)
    {
        //throw new System.NotImplementedException();
    }
}
