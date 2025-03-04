using System;
using Bstick;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Bstick.Common;


public class HapticContorller : Singleton<HapticContorller>
{
    private BstickBridge bstickBridge;

    void Start()
    {
        HapticContorller.Instance.InitHapticDll();
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void InitHapticDll()
    {
        if (bstickBridge != null) return;
        bstickBridge = new BstickBridge();
        //var direction = Convert.ToInt32(BstickDirection.Left);
        bstickBridge.InitializeHapticDevice(OsType.DeskTop, Debug.Log);

        //Callback 함수로 사용
       // bstickBridge.OnParseData();

        SetMotorRange(BstickDirection.Left, BstickSetMotion.MotorRange, new int[5]);
        SetMotorRange(BstickDirection.Right, BstickSetMotion.MotorRange,new int[5]);
    }

    public void SetStiffness(BstickDirection direction, BstickSetMotion motion, int[] stiffness)
    {
        bstickBridge.SendMotionDataChange(direction, motion, new MotionData() { Stiffness = stiffness });
    }

    public void SetMotorRange(BstickDirection direction, BstickSetMotion motion,int[] min)
    {
        bstickBridge.SendMotionDataChange(direction, motion, new MotionData(){MotorMin = min});
    }

    public void SetVibrate(BstickDirection direction, BstickSetMotion motion, VibrateData data)
    {
        bstickBridge.SendMotionDataChange(direction, motion, new MotionData() { VibePattern = data.pattern, VibeRepeat = data.repeat, VibeState = data.state});
    }

    public void SetTempPad(BstickDirection direction, BstickSetMotion motion,bool enable, TempType type, int value)
    {
        bstickBridge.SendMotionDataChange(direction, motion, new MotionData() {TempEnable = enable, TemperaturType = type = type, TempValue = value});
    }

    public List<int> GetBstickGripData(BstickDirection direction, BstickMotion motion)
    {
        return bstickBridge.GetBstickGripData(direction, motion);
    }

    public float GetFingerIndex(BstickDirection direction, BstickMotion motion, FingerIndex idx)
    {
        if (!bstickBridge.IsConnected(direction)) return 0;

        var position =1.0f - ((float)GetBstickGripData(direction, motion)[(int)idx] / Common.MAX_POSITION);

        return position;
    }

    public ushort GetMotorPositionAverage(BstickDirection direction, BstickMotion motion)
    {
        return (ushort)GetBstickGripData(direction, motion).Average(arg => ushort.MinValue);
    }

    public bool GetTouchState(BstickDirection direction, TouchpadState state)
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
    }

    public bool IsConnect(BstickDirection direction)
    {
        return bstickBridge.IsConnected(direction);
    }

    void OnApplicationQuit()
    {
        bstickBridge.CloseClient();
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
