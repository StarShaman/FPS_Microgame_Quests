using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LookAtPlayer : MonoBehaviour
{
    private Transform transformObject;

    private void Start()
    {
        transformObject = GetComponent<Transform>();
    }
    private void Update()
    {
        Vector3 direction = transform.position - Camera.main.transform.position;
        transformObject.rotation = Quaternion.LookRotation(direction);
    }
}
