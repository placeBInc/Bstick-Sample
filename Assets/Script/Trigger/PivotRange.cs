using System;
using System.ComponentModel;
using Bstick;
using UnityEngine;
using static Bstick.Common;

public class PivotRange : MonoBehaviour
{
    [SerializeField] private float pivotRation = 0;

    private TriggerManager triggerManager;
    private int[] pivot = new int[5]; // Number of fingers
    private int[] pushPivot = new int[5];

    void Awake()
    {
        triggerManager = GetComponent<TriggerManager>();
    }

    public void SetPivotRange(int index, float position = 0)
    {
        pivot[index] = pushPivot[index] = (int)(Common.MAX_POSITION * pivotRation);

        //Debug.Log(String.Format("pivot : {0} , {1}", pivot, position));

        triggerManager = GetComponent<TriggerManager>();
        BstickBridge.Instance.SetMotorRange(triggerManager.BstickInfomation().Direction, new MotionData() { MotorMin = pivot });
    }

    public void PushPivotRange(int index)
    {
        pushPivot[index] = 0;
        BstickBridge.Instance.SetMotorRange(triggerManager.BstickInfomation().Direction, new MotionData() { MotorMin = pushPivot });
    }

    public void InitPivot()
    {
        BstickBridge.Instance.SetMotorRange(triggerManager.BstickInfomation().Direction, new Common.MotionData() { MotorMin = new int[5] });
    }

    public void ResetPivot(int index)
    {
        BstickBridge.Instance.SetMotorRange(triggerManager.BstickInfomation().Direction, new Common.MotionData() { MotorMin = pivot });
    }
}
