using System;
using UnityEngine;

public class Stiffness : MonoBehaviour
{
    private TriggerManager triggerManager;

    private float[] stiffness = new float[5]; // Number of fingers

    public float Value = 0.0f;

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
        triggerManager.HapticContorller().SetStiffness(stiffness);
    }

    public void InitStiffness(int index)
    {
        stiffness[index] = 0.0f;
        triggerManager.HapticContorller().SetStiffness(stiffness);
        isStiffnessStay = false;
    }

    public void ApplyStiffness()
    {
        if (isStiffnessStay) return;

        Array.Fill(stiffness, Value);
        triggerManager.HapticContorller().SetStiffness(Value);

        isStiffnessStay = true;
    }
}
