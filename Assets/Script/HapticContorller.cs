using System;
using Bstick;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using static Bstick.Common;
using UnityEngine.XR;

public class HapticContorller : Singleton<HapticContorller>
{
    [SerializeField] private List<BstickInfomation> _bstickInterfaces;
    void Start()
    {
#if　UNITY_EDITOR
        BstickBridge.Instance.InitializeInfo(OsType.DeskTop, Debug.Log);
#else
        BstickBridge.Instance.InitializeInfo(OsType.Aos, Debug.Log);
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {
            BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(new string []{SERVICE_UUID}, (address, name) =>
            {
                if (!name.Contains("Bstick")) return;

                if (name.Contains("L"))
                {
                    Thread.Sleep(1000);
                    if (!address.Contains("54:64:DE:A3:46:53")) return;
                    var bstickInterface = _bstickInterfaces.Where(i => i.Direction == BstickDirection.Left)
                        .SingleOrDefault();
                    BstickBridge.Instance.CreateDevice(BstickDirection.Left, bstickInterface, address);
                }
                else
                {
                    if (!address.Contains("54:64:DE:A3:4A:9D")) return;
                    Thread.Sleep(1000);
                    var bstickInterface = _bstickInterfaces.Where(i => i.Direction == BstickDirection.Right)
                        .SingleOrDefault();
                    BstickBridge.Instance.CreateDevice(BstickDirection.Right, bstickInterface, address);
                }
            });
        }, (error) =>
        {
            Debug.Log(error);
        });
#endif
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void Subscribe(string address, string serviceUuid, string characteristicUuid, BstickDirection direction)
    {
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(address, serviceUuid, characteristicUuid, (s, s1) =>
                    {
                        direction = _bstickInterfaces.Where(i => i.DeivceAddress.Contains(address)).SingleOrDefault().Direction;
                        Debug.Log("start!!!!!!");
                        BstickBridge.Instance.Start(direction);
                    },
                    (s,s1, bytes) => {
                        direction = _bstickInterfaces.Where(i => i.DeivceAddress.Contains(s)).SingleOrDefault().Direction;
                        BstickBridge.Instance.OnParseData(direction, bytes);
                    }
                );
    }


    public void ConnectDevice(BstickDirection direction)
    {
        BstickBridge.Instance.ConnectDevice(direction);
    }

    public void SetStiffness(BstickDirection direction, BstickSetMotion motion, int[] stiffness)
    {
        if (!IsConnect(direction)) return;
        byte[] sendData = new byte[] { };
        BstickBridge.Instance.SetStiffness(direction, new MotionData() { Stiffness = stiffness });
        Debug.Log("SetStiffness ::::::::::::::::::::::" + string.Join(",", sendData));
    }

    public void SetMotorRange(BstickDirection direction, BstickSetMotion motion,int[] min)
    {
        if (!IsConnect(direction)) return;
        byte[] sendData = new byte[] { };
        BstickBridge.Instance.SetStiffness(direction, new MotionData() { MotorMin = min });
    }

    public void SetVibrate(BstickDirection direction, BstickSetMotion motion, VibrateData data)
    {
        if (!IsConnect(direction)) return;
        byte[] sendData = new byte[] { };
        BstickBridge.Instance.SetVibrator(direction, new MotionData() { VibePattern = data.pattern, VibeRepeat = data.repeat, VibeState = data.state});
    }

    public void SetTempPad(BstickDirection direction, BstickSetMotion motion,bool enable, TempType type, int value)
    {
        if (!IsConnect(direction)) return;
        byte[] sendData = new byte[] { };
        BstickBridge.Instance.SetStiffness(direction, new MotionData() { TempEnable = enable, TemperaturType = type , TempValue = value });
    }

    public List<int> GetBstickGripData(BstickDirection direction, BstickMotion motion)
    {
        if (!IsConnect(direction)) return new List<int>(5);
        return BstickBridge.Instance.GetBstickGripData(direction, motion);
    }

    public float GetFingerIndex(BstickDirection direction, BstickMotion motion, FingerIndex idx)
    {
        if (!BstickBridge.Instance.IsConnected(direction)) return 0;
        
        var position =1.0f - ((float)GetBstickGripData(direction, motion)[(int)idx] / Common.MAX_POSITION);
        
        return position;
    }

    public ushort GetMotorPositionAverage(BstickDirection direction, BstickMotion motion)
    {
        if (!IsConnect(direction)) return 0;
        return (ushort)GetBstickGripData(direction, motion).Average(arg => ushort.MinValue);
    }

    /*public bool GetTouchState(BstickDirection direction, TouchpadState state)
    {
        return bstickBridge.GetTouchState(direction,state);
    }

    public bool GetTouchpadState(BstickDirection direction, TouchpadState state)
    {
        return bstickBridge.GetTouchpadState(direction, state);
    }

    public TouchpadState GetTouchStateCheck(BstickDirection direction)
    {
        return bstickBridge.GetTouchStateCheck(direction);
    }*/

    public bool IsConnect(BstickDirection direction)
    {
        return BstickBridge.Instance.IsConnected(direction);
    }

    void OnMessage(string msg)
    {
        Debug.Log("[BLE] " + msg);
        if (!msg.Contains("Bstick")) return;
        /*if (msg.Contains("BluetoothLE#BluetoothLE00:1a:7d:da:71:13-54:64:de:a3:4a:9d"))
        {
            var bstickInterface = _bstickInterfaces.Where(i => i.Direction == BstickDirection.Left)
                .SingleOrDefault();
            BstickBridge.Instance.CreateDevice(BstickDirection.Left, bstickInterface, "BluetoothLE#BluetoothLE00:1a:7d:da:71:13-6c:1d:eb:ae:7e:da");
        }
        else*/ if (msg.Contains("BluetoothLE#BluetoothLE00:1a:7d:da:71:13-54:64:de:a3:4a:9d"))
        {
            Thread.Sleep(1000);
            var bstickInterface = _bstickInterfaces.Where(i => i.Direction == BstickDirection.Right)
                .SingleOrDefault();
            BstickBridge.Instance.CreateDevice(BstickDirection.Right, bstickInterface, "BluetoothLE#BluetoothLE00:1a:7d:da:71:13-54:64:de:a3:4a:9d");
        }
    }

    void OnApplicationQuit()
    {
        BstickBridge.Instance.CloseClient(BstickDirection.Right);
        BstickBridge.Instance.CloseClient(BstickDirection.Left);
    }

    [Serializable]
    public class VibrateData
    {
        [Range(1, 100)]
        public int[] pattern;
        public int repeat;
        public VibrateState state;
    }
}
