using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    [SerializeField] private HapticContorller hapticContorller;

    private Stiffness stiffness;
    private PivotRange pivotRange;

    void Awake()
    {
        stiffness = GetComponent<Stiffness>();
        pivotRange = GetComponent<PivotRange>();
    }

    public HapticContorller HapticContorller()
    {
        return hapticContorller;
    }

    public Stiffness Stiffness()
    {
        return stiffness;
    }

    public PivotRange PivotRange()
    {
        return pivotRange;
    }
}
