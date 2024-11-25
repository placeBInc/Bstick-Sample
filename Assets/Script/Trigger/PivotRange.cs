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
        pivot[index] = pushPivot[index] = (int)((1.0f - position) * Common.MAX_POSITION * pivotRation);

        //Debug.Log(String.Format("pivot : {0} , {1}", pivot, position));

        triggerManager.HapticContorller().SetMotorRange(index, pivot);
    }

    public void PushPivotRange(int index)
    {
        pushPivot[index] = 0;
        triggerManager.HapticContorller().SetMotorRange(index, pushPivot);
    }

    public void InitPivot()
    {
        triggerManager.HapticContorller().SetMotorRange(0);
    }

    public void ResetPivot(int index)
    {
        triggerManager.HapticContorller().SetMotorRange(index, pivot);
    }
}
