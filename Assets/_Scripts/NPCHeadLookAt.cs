using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NPCHeadLookAt : MonoBehaviour
{
    [SerializeField] private Rig rig;
    [SerializeField] private Transform headLookAtTransform;
    [SerializeField] private MultiAimConstraint multiAimConstraint;

    private bool isLookingAtPosition;

    private void Update()
    {
        float targetWeight = isLookingAtPosition ? 1f : 0f;
        float lerpSpeed = 2f;
        rig.weight = Mathf.Lerp(rig.weight, targetWeight, Time.deltaTime * lerpSpeed);

        if (!isLookingAtPosition && Mathf.Approximately(rig.weight, 0f))
        {
            ResetLookAtSource();
        }
    }

    public void LookAtPosition(Vector3 lookAtPosition)
    {
        isLookingAtPosition = true;
        headLookAtTransform.position = lookAtPosition;
        var constraintData = multiAimConstraint.data.sourceObjects;
        if (constraintData.Count > 0)
        {
            constraintData.SetTransform(0, headLookAtTransform);
        }
        else
        {
            constraintData.Add(new WeightedTransform(headLookAtTransform, 1f));
        }
        multiAimConstraint.data.sourceObjects = constraintData;
    }
    public void StopLooking()
    {
        isLookingAtPosition = false;
    }
    private void ResetLookAtSource()
    {
        var constraintData = multiAimConstraint.data.sourceObjects;
        if (constraintData.Count > 0)
        {
            constraintData.Clear();
            multiAimConstraint.data.sourceObjects = constraintData;
        }
    }

}
