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

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

    }
    private void FixedUpdate()
    {
        x = Input.GetAxis("Vertical");
        z = Input.GetAxis("Horizontal");

        rigidbody.AddForce(x * speed, 0, -z * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Destroy(other.gameObject);
        //    particle = other.gameObject.GetComponent<ParticleSystem>();
        //    particle.IsAlive(true);

            gameController.itemCount = GameObject.FindGameObjectsWithTag("Item").Length;
            gameController.countText.text = gameController.itemCount.ToString();
        }
    }
       
    IEnumerator Starter()
    {
        yield return new WaitForSeconds(1);
    }
}
