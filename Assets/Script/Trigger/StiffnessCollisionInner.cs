using System;
using System.Reflection;
using UnityEngine;

public class StiffnessCollisionInner : MonoBehaviour
{
    [SerializeField] TriggerManager triggerManager;
    [SerializeField] private float delayTime = 0.0f;

    [SerializeField] private int colFingerCount = 0;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Inner Enter ");
        var otherName = other.name;
        if (otherName.Contains("Outer")) return;

        var finger = other.GetComponent<FingerInfo>();

        if (finger == null) return;

        var index = finger.GetFingerIndex();

        triggerManager.Stiffness().SetStiffness(index);
        colFingerCount++;
    }

    void OnTriggerStay(Collider other)
    {
        var finger = other.GetComponent<FingerInfo>();

        if (finger == null) return;

        if (finger.PivotStay) return;

        if (!finger.CalculateStayTime(delayTime)) return;

        var index = finger.GetFingerIndex();
        triggerManager.PivotRange().PushPivotRange(index);
        finger.PivotStay = true;
    }

    void OnTriggerExit(Collider other)
    {
        colFingerCount--;

        var finger = other.GetComponent<FingerInfo>();

        if (finger == null) return;

        finger.InitStay();

        triggerManager.PivotRange().ResetPivot(finger.GetFingerIndex());
        triggerManager.Stiffness().InitStiffness(finger.GetFingerIndex());
        
        //Debug.Log(String.Format("Exit"));
    }
}
