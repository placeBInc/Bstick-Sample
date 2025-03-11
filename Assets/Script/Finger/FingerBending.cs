using System;
using System.Collections.Generic;
using System.Linq;
using Bstick;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Bstick.Common;


public class FingerBending : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    public float BendAmount = 0.1f; // Adjust the amount as needed
    public float BendAngle = 40.0f; // Adjust the angle as needed Other Finger : 40
    public float BendThumbAngle = 20.0f; // Adjust the angle as needed Thumb Finger : 20

    // Assign the finger joint transforms in the Inspector
    public Transform[] ThumbJoints;
    public Transform[] IndexJoints; 
    public Transform[] MiddleJoints; 
    public Transform[] RingJoints; 
    public Transform[] PinkyJoints;

    private Quaternion[] initialThumbRotations;
    private Quaternion[] initialIndexRotations;
    private Quaternion[] initialMiddleRotations;
    private Quaternion[] initialRingRotations;
    private Quaternion[] initialPinkyRotations;

    private BstickInfomation bstickInfomation;

    private readonly string[] prefixGroup = new[] { "T", "I", "M", "R", "P" };
    private readonly string[] suffixGroup = new[] { "MCP", "PIP", "DIP", "TIP" };

    void Awake()
    {
        //bstickInfomation = GetComponent<BstickInfomation>();

        //if(hapticContorller == null ) Debug.Log("Bstick Not Found.");
    }

    void Start()
    {
        // Store the initial positions of the finger joints
        initialThumbRotations = new Quaternion[ThumbJoints.Length];
        initialIndexRotations = new Quaternion[IndexJoints.Length];
        initialMiddleRotations = new Quaternion[MiddleJoints.Length];
        initialRingRotations = new Quaternion[RingJoints.Length];
        initialPinkyRotations = new Quaternion[PinkyJoints.Length];

        for (var i = 0; i < suffixGroup.Length; i++)
        {
            initialThumbRotations[i] = ThumbJoints[i].localRotation;
            initialIndexRotations[i] = IndexJoints[i].localRotation;
            initialMiddleRotations[i] = MiddleJoints[i].localRotation;
            initialRingRotations[i] = RingJoints[i].localRotation;
            initialPinkyRotations[i] = PinkyJoints[i].localRotation;
        }
    }

    void Update()
    {
        //if (hapticContorller == null) return;

        /*var motion = Common.BstickMotion.Position;

        BendFingers(initialThumbRotations, ThumbJoints, 
            -HapticContorller.Instance.GetFingerIndex(bstickInfomation.Direction, motion, FingerIndex.Thumb) * BendThumbAngle);
        BendFingers(initialIndexRotations, IndexJoints, 
            -HapticContorller.Instance.GetFingerIndex(bstickInfomation.Direction, motion, FingerIndex.Index) * BendAngle);
        BendFingers(initialMiddleRotations, MiddleJoints, 
            -HapticContorller.Instance.GetFingerIndex(bstickInfomation.Direction, motion, FingerIndex.Middle) * BendAngle);
        BendFingers(initialRingRotations, RingJoints, 
            -HapticContorller.Instance.GetFingerIndex(bstickInfomation.Direction, motion, FingerIndex.Ring) * BendAngle);
        BendFingers(initialPinkyRotations, PinkyJoints, -
            HapticContorller.Instance.GetFingerIndex(bstickInfomation.Direction, motion, FingerIndex.Pinky)* BendAngle);*/
    }

    void BendFingers(Quaternion[] initRotation, Transform[] joints, float position)
    {
        for (var i = 0; i < joints.Length; i++)
        {
            // Move the finger joint downwards to bend the finger
            var bendRotation = initRotation[i] * Quaternion.Euler(0, 0, initRotation[i].z + position * (i==0 ? BendAmount : 1.0f));
            joints[i].localRotation = bendRotation;
        }
    }

    void FlatIndexFinger()
    {
        IndexJoints[1].localRotation = initialIndexRotations[1] * Quaternion.Euler(0, 0, initialIndexRotations[1].z + BendAngle);
        IndexJoints[2].localRotation = initialIndexRotations[2] * Quaternion.Euler(0, 0, initialIndexRotations[2].z + BendAngle);
    }

    void ResetFingers()
    {
        //for (int i = 0; i < fingerJoints.Length; i++)
        //{
        //    // Reset the finger joint to its initial position
        //    fingerJoints[i].localPosition = initialPositions[i];
        //    fingerJoints[i].localRotation = initialRotations[i];
        //}
    }

    public void BendFingers(List<int> position)
    {
        var parsePosition = position.Select(x => 1.0f - (float)x / Common.MAX_POSITION).ToList();
        BendFingers(initialThumbRotations, ThumbJoints,
            -parsePosition[(int)FingerIndex.Thumb] * BendThumbAngle);
        BendFingers(initialIndexRotations, IndexJoints,
            -parsePosition[(int)FingerIndex.Index] * BendAngle);
        BendFingers(initialMiddleRotations, MiddleJoints,
            -parsePosition[(int)FingerIndex.Middle] * BendAngle);
        BendFingers(initialRingRotations, RingJoints,
            -parsePosition[(int)FingerIndex.Ring] * BendAngle);
        BendFingers(initialPinkyRotations, PinkyJoints, -
            parsePosition[(int)FingerIndex.Pinky] * BendAngle);
    }

    public void MappingFinger()
    {
        foreach (var prefix in prefixGroup)
        {
            switch (prefix)
            {
                case "T":
                    SetFingerBranch(prefix,4, ref ThumbJoints);
                    break;
                case "I":
                    SetFingerBranch(prefix,0, ref IndexJoints);
                    break;
                case "M":
                    SetFingerBranch(prefix,1, ref MiddleJoints);
                    break;
                case "R":
                    SetFingerBranch(prefix,3, ref RingJoints);
                    break;
                case "P":
                    SetFingerBranch(prefix,2, ref PinkyJoints);
                    break;
            }
        }
    }

    private void SetFingerBranch(string prefix,int index, ref Transform[] list)
    {
        var start = parent.transform.GetChild(index);
        foreach (var suffix in suffixGroup.Select(((s, i) => (s, i))))
        {
            var branch = String.Format("{0}{1}", prefix, suffix.s);

            if (suffix.i == 0)
                list[suffix.i] = start;
            else
            {
                var child = start.GetChild(0);

                if (child == null) continue;
                list[suffix.i] = child.transform;
                start = child;
            }
        }
    }
}