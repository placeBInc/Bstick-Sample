using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using Bstick;
using PimDeWitte.UnityMainThreadDispatcher;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using static Bstick.Common;
public class BstickInfomation : MonoBehaviour, IBstickInterface
{
    public BstickDirection Direction;
    public BstickEnable BstickEnable;
    public string DeivceAddress;

    private BstickKey BstickKeyStatus { get; set; } = BstickKey.Release;
    private FingerBending _bending;
    private Action<string, string, string> connectAction;
    private delegate void OnDataparse(BstickDirection direction, byte[] data);

    void Awake()
    {
        _bending = GetComponent<FingerBending>();
    }
    void Start()
    {
#if UNITY_EDITOR
        BstickBridge.Instance.CreateDevice(Direction, this);
        //BstickBridge.Instance.InitWriteCsv(true);
#endif
    }

    public void CreateDevice(bool success, string address = "")
    {
        Debug.Log("Connect Device Address : " + address);
#if UNITY_EDITOR
        BstickBridge.Instance.ConnectDevice(Direction);
        //connectAction += BstickBridge.Instance.ConnectDevice;
#else
        connectAction += BstickBridge.Instance.ConnectDevice;
        BluetoothLEHardwareInterface.ConnectToPeripheral(address, null, null, connectAction);
#endif
    }
    public void AddDeviceAddress(string address)
    {
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

    public void Subscribe(string address, string serviceUuid, string characteristicUuid)
    {
        Debug.Log(string.Format("address: {0}, sUuid : {1}, cUuid : {2}", address, serviceUuid, characteristicUuid));
        //mt.text += characteristicUuid + "\n";
        if (CHARACTERISTIC_UUID != characteristicUuid.ToUpper()) return;
        DeivceAddress = address;
        HapticContorller.Instance.Subscribe(address, serviceUuid, characteristicUuid, Direction);
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
        var preKey = BstickKeyStatus;
        var key = data[0];
        if (preKey == BstickKey.Up)
            BstickKeyStatus = BstickKey.Release;
        if (preKey == BstickKey.Down && key == (int)BstickKey.Up)
            BstickKeyStatus = BstickKey.Up;
        else if (key == (int)BstickKey.Down)
            BstickKeyStatus = BstickKey.Down;

        Debug.Log("Bstick Button Staus" + key.ToString());

        //throw new System.NotImplementedException();
    }
    public void SendToQuaternion(List<float> data)
    {
        Debug.Log("SendToQuaternion : " + string.Join(",", data));
        //throw new System.NotImplementedException();
        return;
        /*float w = data[0];
        float x = data[1];
        float y = data[2];
        float z = data[3];*/
        float x = data[0];
        float y = data[1];
        float z = data[2];
        float w = data[3];

        var rawIMU = new Quaternion(x, y, z, w);
        //lock (lockObject)
        /*{
            // 1. 좌표계 보정 (ESP32 기준 Z, W 반전)
            if (applyZAxisCorrection)
                rawIMUQuat = new Quaternion(rawIMU.x, rawIMU.y, -rawIMU.z, -rawIMU.w);
        }*/

        UnityMainThreadDispatcher.Instance().Enqueue(() => transform.rotation = rawIMU);
    }
    public void SendToIMUArray(List<float> data)
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

    #region AOS_IOS
    public void SendCommand(byte[] cmd, string address)
    {
        Thread.Sleep(10);
        BluetoothLEHardwareInterface.WriteCharacteristic(address, SERVICE_UUID, CHARACTERISTIC_UUID, cmd, cmd.Length, false, null);
    }

    public void SendCommand(byte[] cmd, string address, BstickEnable enable)
    {
        Thread.Sleep(10);
        BluetoothLEHardwareInterface.WriteCharacteristic(address, SERVICE_UUID, CHARACTERISTIC_UUID, cmd, cmd.Length, false, null);

        BstickEnable = enable;
        if (enable == BstickEnable.Start)
        {
            Debug.Log("Touch!!!!");
            BstickBridge.Instance.EnableTouch(Direction);
        }
    }

    public bool GetBstickKey(BstickKey key)
    {
        Debug.Log($"[Status] : {BstickKeyStatus.ToString()}");
        return BstickKeyStatus == key;
    }

    public void SendErrorMessage(string message)
    {
        //throw new NotImplementedException();
    }

    void OnApplicationQuit()
    {
#if UNITY_ANDROID
        BluetoothLEHardwareInterface.UnSubscribeCharacteristic(DeivceAddress, Common.SERVICE_UUID, Common.CHARACTERISTIC_UUID,
            s => { Debug.Log("UnSubscribe"); });
#endif
    }
    #endregion
}
