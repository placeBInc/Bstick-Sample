using UnityEngine;

public class FingerInfo : MonoBehaviour
{
    // Thumb : 0, Index : 1, Middle : 2, Ring : 3, Pinky : 4

    [SerializeField] private int Index = 0;

    public bool StiffnessEnter { get; set; }
    public bool StiffnessStay { get; set; }
    public bool PivotEnter { get; set; }
    public bool PivotStay { get; set; }

    private float prevTime = 0;
    public int GetFingerIndex()
    {
        StiffnessEnter = true;
        return Index;
    }

    public void InitStay()
    {
        prevTime = 0;
        PivotStay = false;
    }

    public bool CalculateStayTime(float delay)
    {
        if(prevTime == 0)
            prevTime = Time.deltaTime;
        else
        {
            prevTime += Time.deltaTime;
        }

        var result = delay <= prevTime;

        return result;
    }
}
