using System;
using System.ComponentModel;
using Bstick;
using UnityEngine;

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
        HapticContorller.Instance.SetMotorRange(triggerManager.BstickInfomation().direction, Common.BstickSetMotion.MotorRange, pivot);
    }

    public void PushPivotRange(int index)
    {
        pushPivot[index] = 0;
        HapticContorller.Instance.SetMotorRange(triggerManager.BstickInfomation().direction, Common.BstickSetMotion.MotorRange, pushPivot);
    }

    public void InitPivot()
    {
        HapticContorller.Instance.SetMotorRange(triggerManager.BstickInfomation().direction, Common.BstickSetMotion.MotorRange, new int[5]);
    }

    public void ResetPivot(int index)
    {
        HapticContorller.Instance.SetMotorRange(triggerManager.BstickInfomation().direction, Common.BstickSetMotion.MotorRange, pivot);
    }
}
