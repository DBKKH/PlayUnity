using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TestGetArgs : MonoBehaviour
{
    private Text argText;
    private string resultArgs = "None";

    private void Awake()
    {
        argText = GetComponent<Text>();
    }

    private void Start()
    {
#if UNITY_STANDALONE_WIN
        resultArgs = "";
        var args = Environment.GetCommandLineArgs();

        if (args.Length >= 10)
        {
            argText.text += "Maximam = 10 args";
            return;
        }

        int i = 0;
        while (i < args.Length)
        {
            resultArgs += i.ToString();
            resultArgs += ":";
            resultArgs += args[i];
            resultArgs += ", ";
        }
#endif

        argText.text += resultArgs;
    }
}