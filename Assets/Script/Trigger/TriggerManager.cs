using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    [SerializeField] private HapticContorller hapticContorller;

    public bool UseVibrator = false;

    private Stiffness stiffness;
    private PivotRange pivotRange;
    private Vibrator vibrator;

    void Awake()
    {
        stiffness = GetComponent<Stiffness>();
        pivotRange = GetComponent<PivotRange>();
        vibrator = GetComponent<Vibrator>();
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

    public Vibrator Vibrator()
    {
        return vibrator;
    }
}
