using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [HideInInspector] public int itemCount;
    public Text countText, elapsedText, resultText;
	public GameObject playerObject;

    public string win = "You Win!", lose = "You Lose...";
    public float elapsedTime = 5;

	[SerializeField,Tooltip("wanna use elapsed time")] bool NotLoser = true;

    private void Awake()
    {
		if (NotLoser) elapsedTime = 0;
		resultText.gameObject.SetActive(false);
    }

    private void Update()
    {
		itemCount = GameObject.FindGameObjectsWithTag("Item").Length;
		countText.text = itemCount.ToString();

		if (NotLoser) Elasper();
		else UseLoser();

		elapsedText.text = elapsedTime.ToString();
	}

	/// <summary>
	/// Get and culculate elapsed time. 
	/// </summary>
	void Elasper()
	{
		if (itemCount <= 0)
		{
			WinProcess();
			Result();
			return;
		}
		elapsedTime += Time.deltaTime;
	}

	/// <summary>
	/// Use Time Limit.
	/// </summary>
	void UseLoser()
	{
		elapsedTime -= Time.deltaTime;

		if (elapsedTime <= 0.0f) Result();
	}

    public  void Result()
    {
        //elapsedText.gameObject.SetActive(false);

        if (itemCount == 0) WinProcess();
        else if (itemCount != 0) LoseProcess();
        else Debug.LogError("Can't detect winner");

        resultText.gameObject.SetActive(true);
    }

    public void WinProcess()
    {
        resultText.text = this.win;
        resultText.color = Color.red;
    }

    public void LoseProcess()
    {
		playerObject.SetActive(false);
        resultText.text = this.lose;
        resultText.color = Color.blue;
    }
}
