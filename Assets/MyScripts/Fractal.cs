using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
	[SerializeField] bool spawnHere = true, spawnIncrimental = false;
	[SerializeField] Mesh mesh_;
	[SerializeField] Material material_;

	[SerializeField] float updatePos = 0.5f, width_ = 0.1f;
	[SerializeField] int depth_ = 0;


	const int maxDepth = 7;
	Vector3 start_ = Vector3.zero;
	Vector3 end_ = Vector3.up;
    List<GameObject> objects = new List<GameObject>();

	private void Awake()
	{
		if (spawnHere)
		{
			start_ = this.transform.position;
			end_ += start_;
		}
	}

	void initialize(Fractal parent, float giveLength, float giveWidth, Quaternion giveRotation, int depth)
	{
		this.mesh_ = parent.mesh_;
		this.material_ = parent.material_;
		this.transform.position = parent.transform.position + Vector3.up;
		this.start_ = parent.end_;
		this.end_ = start_ + giveRotation * (Vector3.up * giveLength);
		this.depth_ = depth;
	}


	IEnumerator Start()
	{
	//	if (spawnIncrimental)
		{
			start_ = this.transform.position;
			end_ += start_;
		}

		gameObject.AddComponent<MeshFilter>().mesh = mesh_;
		gameObject.AddComponent<MeshRenderer>().material = material_;
		transform.position = (start_ + end_) * updatePos;
		var diff = end_ - start_;
		var length = diff.magnitude;

		transform.rotation = Quaternion.LookRotation(diff) * Quaternion.Euler(90f, 0f, 0f);
		transform.localScale = new Vector3(width_, length * 1.0f, width_);

		if (depth_ >= maxDepth) yield break;

		float roty = Random.Range(0f, 360f);

		for (var i = 0; i < 5; ++i)
		{
			//Debug.Log(i);
			if (Random.Range(0, 2) != 0) continue;
			yield return new WaitForSeconds(0.1f);
			var child = new GameObject();
			child.name = gameObject.name;

            objects.Add(child);

			var fractal = child.AddComponent<Fractal>();
			fractal.initialize(this, length * 0.75f, width_ * 0.75f, transform.rotation * Quaternion.Euler(30f, roty, 0f), depth_ + 1);
			roty += 137.5f;
		}

        //Debug.Log(objects.ToArray().Length);
        //Debug.Log(objects.ToArray().LongLength);

    }

}
