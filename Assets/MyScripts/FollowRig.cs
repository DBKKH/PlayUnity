using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRig : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;

    private void Awake()
    {
        offset = this.GetComponent<Transform>().position - target.position;
    }

    private void Update()
    {
        GetComponent<Transform>().position = target.position + offset;
    }

}
