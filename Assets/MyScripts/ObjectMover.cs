using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
	public float moveRangeX = 5, moveRangeY = 0, moveRangeZ = 1;
	float sumTime = 0.1f;

	private void Update()
	{
			sumTime += Time.deltaTime;
			this.transform.position = new Vector3(Mathf.Sin(180 * sumTime * Mathf.Deg2Rad), this.transform.position.y, this.transform.position.z);
			//this.transform.position += new Vector3(Mathf.PingPong(Time.time, moveRangeX), Mathf.PingPong(Time.time, moveRangeY), Mathf.PingPong(Time.time, moveRangeZ));
		}
}
