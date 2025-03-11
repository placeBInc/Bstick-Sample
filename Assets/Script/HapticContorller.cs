using System;
using Bstick;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Bstick.Common;


public class HapticContorller : Singleton<HapticContorller>
{
    void Start()
    {
        BstickBridge.Instance.InitializeInfo(OsType.DeskTop, Debug.Log);
    }
    // Update is called once per frame
    void Update()
    {
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
