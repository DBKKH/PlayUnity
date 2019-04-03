///http://ftvoid.com/blog/post/823
///Refer above URL
using System.Collections;
using UnityEngine;

public class PraCountYeild : MonoBehaviour
{
	IEnumerator Start()
	{
		int count = 0;

		while (true){
			Debug.Log(count++);

			yield return new WaitForSeconds(1);
		}
	}
}
