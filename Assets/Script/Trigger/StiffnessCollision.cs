using UnityEngine;

public class StiffnessCollision : MonoBehaviour
{
    [SerializeField] TriggerManager triggerManager;
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        /*var name = other.gameObject.name;
        Debug.Log("Stiffness Trigger!" + name);*/

        var finger = other.GetComponent<FingerInfo>();

        if (finger == null) return;

        var index = finger.GetFingerIndex();
 
        triggerManager.PivotRange().SetPivotRange(index);
    }
}
