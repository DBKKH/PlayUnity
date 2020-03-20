using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMethods : MonoBehaviour
{

    public void Test(int i_arg)
    {
        Debug.LogFormat("Test i_arg={0}", i_arg);
    }

    public void Test(float i_arg)
    {
        Debug.LogFormat("Test i_arg={0}", i_arg);
    }

    public void Test(string i_arg)
    {
        Debug.LogFormat("Test i_arg={0}", i_arg);
    }
}