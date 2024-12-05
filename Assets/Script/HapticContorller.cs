using System;
using Bstick;
using System.Collections.Generic;
using UnityEngine;
using static Bstick.Common;

public class HapticContorller : MonoBehaviour
{
    private BstickBridge bstickBridge;
    public HapticDirection HapticDir = HapticDirection.Right;

    public bool IsLeft = false;

    public enum HapticDirection
    {
        Left = 1, Right = 2
    }

// Start is called before the first frame update
    void Start()
    {
        InitHapticDll();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void InitHapticDll()
    {
        if (bstickBridge != null) return;
        bstickBridge = new BstickBridge();
        var direction = Convert.ToInt32(HapticDir);
        bstickBridge.InitializeHapticDevice(direction, Debug.Log);
        SetMotorRange(0);
    }

    public void SetStiffness(float stiffness)
    {
        bstickBridge.SetStiffness(stiffness);
    }

    public void SetStiffness(float[] stiffness)
    {
        bstickBridge.SetStiffness(stiffness);
    }

    public void SetMotorRange(int min = 0)
    {
        bstickBridge.SetMotorRange(min);
    }

    public void SetMotorRange(int index, int[] min)
    {
        bstickBridge.SetMotorRange(index, min);
    }

    public void SetVibrate(VibrateData data)
    {
        bstickBridge.SetVibrateBstick(data.pattern, data.repeat, data.state);
    }

    public void SetTempPad(bool enable, TempType type, int value)
    {
        bstickBridge.SetTempPadBstick(enable, type, value);
    }

    public List<float> GetMotorPosition()
    {
        return bstickBridge.GetMotorPosition();
    }

    public List<float> GetPressureValues()
    {
        return bstickBridge.GetPressureValues();
    }

    public float GetFingerIndex(int idx)
    {
        if (!bstickBridge.IsConnected()) return 0;

        var position = bstickBridge.GetMotorPosition()[idx];

        return position;
    }

    public float GetMotorPositionAverage()
    {
        return bstickBridge.GetAverageMotorPosition();
    }

    public bool TouchButtonPress()
    {
        return bstickBridge.TouchButtonPress();
    }

    public bool TouchButtonRelease()
    {
        return bstickBridge.TouchButtonRelease();
    }

    public bool GetTouchState(TouchpadState state)
    {
        return bstickBridge.GetTouchState(state);
    }

    public bool GetTouchpadState(TouchpadState state)
    {
        return bstickBridge.GetTouchpadState(state);
    }

    public TouchpadState GetTouchStateCheck()
    {
        return bstickBridge.GetTouchStateCheck();
    }

    public bool IsConnect()
    {
        return bstickBridge.IsConnected();
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
