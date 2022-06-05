using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRig : MonoBehaviour
{
    public Transform target;
    Vector3 offset;

    void Awake()
    {
        offset = this.GetComponent<Transform>().position - target.position;
    }

    void Update()
    {
        GetComponent<Transform>().position = target.position + offset;
    }

}
