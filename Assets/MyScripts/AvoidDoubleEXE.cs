using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvoidDoubleEXE : MonoBehaviour
{
	public Button endButton;
	public Text launchDoubleAppLog;
	public string errorLog = "がすでにが起動しています";

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	void Init()
	{
		launchDoubleAppLog.gameObject.SetActive(false);
		endButton.gameObject.SetActive(false);

		var appName = Application.productName;
		errorLog += appName;
		launchDoubleAppLog.text = errorLog;

		if (System.Diagnostics.Process.GetProcessesByName(appName).GetUpperBound(0) > 0)
		{
			launchDoubleAppLog.gameObject.SetActive(true);
			endButton.gameObject.SetActive(true);

			Debug.Log(errorLog);
			endButton.onClick.AddListener(()=>Application.Quit());			
		}
		else
		{
			Debug.unityLogger.logEnabled = false;
		}
	}
}
