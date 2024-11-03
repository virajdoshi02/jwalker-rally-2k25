using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorHit : MonoBehaviour
{
    private bool Smacked = false;
    private float spinPower = 1.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (Smacked)
        {
            return;
        }

        if (collision.gameObject.tag == "Car")
        {
            // fly to space bruh
            Smacked = true;
            Rigidbody rb = this.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;

            if (rb)
            {
                Vector3 v = collision.relativeVelocity;
                Vector3 rand;

                rand.x = Random.Range(-1.0f, 1.0f);
                rand.y = Random.Range(-1.0f, 1.0f);
                rand.z = Random.Range(-1.0f, 1.0f);

                rb.AddForce(v * (9.2f * spinPower));
                rb.AddTorque(v + rand * 800.0f);
            }

        }
    }
    void Start()
    {
        spinPower = Random.Range(3.0f, 9.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
