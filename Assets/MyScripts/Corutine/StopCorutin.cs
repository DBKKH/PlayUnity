using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StopCorutin : MonoBehaviour 
{
	[SerializeField] UnityEngine.UI.Text text;
	[SerializeField] float waitTime = 3;
 float clickTime = 0;

	IEnumerator coroutineMethod;
	
	void Start()
	{
		// IEnumeratorを取得する
		coroutineMethod = ChangeColorCoroutine();
	}
	
	void OnMouseDown()
	{
		// コルーチンをIEnumeratorの位置から開始（再開）
		StartCoroutine(coroutineMethod);
	}
	
	void OnMouseUp()
	{
		// コルーチンを停止
		StopCoroutine(coroutineMethod);
	}
	
	IEnumerator ChangeColorCoroutine()
	{
		// コルーチン進行中は数値を増加させる
		while( clickTime < waitTime ){
			clickTime += Time.deltaTime;
			text.text = clickTime.ToString("0.000");
			yield return null;
		}
		text.text = "finished";
	}
}