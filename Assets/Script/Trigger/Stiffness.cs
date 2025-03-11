using System;
using Bstick;
using UnityEngine;
using static Bstick.Common;

public class Stiffness : MonoBehaviour
{
    private TriggerManager triggerManager;

    private int[] stiffness = new int[5]; // Number of fingers

    public int Value = 0;

    private bool isStiffnessStay = false;

    void Awake()
    {
        triggerManager = GetComponent<TriggerManager>();
    }
    // Start is called before the first frame update
    void Update()
    {
    }

    public void SetStiffness(int index)
    {
        stiffness[index] = Value;
        BstickBridge.Instance.SetStiffness(triggerManager.BstickInfomation().Direction, new MotionData(){Stiffness = stiffness });
    }

    public void InitStiffness(int index)
    {
        stiffness[index] = 0;
        BstickBridge.Instance.SetStiffness(triggerManager.BstickInfomation().Direction, new MotionData() { Stiffness = stiffness });
        isStiffnessStay = false;
    }

    public void ApplyStiffness()
    {
        if (isStiffnessStay) return;

        Array.Fill(stiffness, Value);
        BstickBridge.Instance.SetStiffness(triggerManager.BstickInfomation().Direction, new MotionData() { Stiffness = stiffness });

        isStiffnessStay = true;
    }

    public void ApplyStiffnessEdit()
    {
        Array.Fill(stiffness, Value);
        BstickBridge.Instance.SetStiffness(triggerManager.BstickInfomation().Direction, new MotionData() { Stiffness = stiffness });
    }
}
