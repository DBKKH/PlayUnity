using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [HideInInspector] public int itemCount;
    public Text countText, elapsedText, resultText;

    public string win = "You Win!", lose = "You Lose...";
    public float elapsedTime = 5;

    private void Awake()
    {
        resultText.gameObject.SetActive(false);

        itemCount = GameObject.FindGameObjectsWithTag("Item").Length;
        countText.text = itemCount.ToString();
    }

    private void Update()
    {
        elapsedTime -= Time.deltaTime;
        elapsedText.text = elapsedTime.ToString();

        if (elapsedTime == 0.0f) Result();
    }

    public  void Result()
    {
        elapsedText.gameObject.SetActive(false);

        if (itemCount == 0) WinProcess();
        else if (itemCount != 0) LoseProcess();
        else Debug.LogError("Can't detect winner");

        resultText.gameObject.SetActive(true);
    }

    void WinProcess()
    {
        resultText.text = this.win;
        resultText.color = Color.red;
    }

    void LoseProcess()
    {
        resultText.text = this.lose;
        resultText.color = Color.blue;
    }
}
