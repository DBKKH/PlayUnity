using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour
{
	[SerializeField] Vector3 rotation = new Vector3(0, 20, 0);
	[SerializeField] Space space;

	void Update()
	{
		float d = Time.deltaTime;
		transform.Rotate(rotation * d, space);
	}
}
