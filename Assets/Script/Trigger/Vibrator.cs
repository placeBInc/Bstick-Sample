using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static HapticContorller;

[ExecuteInEditMode]
public class Vibrator : MonoBehaviour
{
    [SerializeField] private VibrateData vibrateData;

    private TriggerManager triggerManager;
    private int patternMax = 20;
    void Awake()
    {
        triggerManager = GetComponent<TriggerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVibrator()
    {
        triggerManager.HapticContorller().SetVibrate(vibrateData);
    }

    public void OnValidate()
    {
        var length = vibrateData.pattern.Length;

        if (length > patternMax)
        {
            var overPattern = vibrateData.pattern;
            vibrateData.pattern = overPattern.Take(patternMax).ToArray();
            Debug.Log(string.Format("The current Pattern lenght is Over {0}", patternMax));
        }
    }
}
