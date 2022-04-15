using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameController gameController;
    public float speed = 10.0f;
    float x, z;
    Rigidbody rigidbody;

    ParticleSystem particle;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        x = Input.GetAxis("Vertical");
        z = Input.GetAxis("Horizontal");

        rigidbody.AddForce(x * speed, 0, -z * speed);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Destroy(other.gameObject);
        }
		else if (other.CompareTag("Disturber"))
		{
			gameController.Result();	
		}
    }
       
    IEnumerator Starter()
    {
        yield return new WaitForSeconds(1);
    }
}
