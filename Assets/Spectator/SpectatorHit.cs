using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorHit : MonoBehaviour
{
    private bool Smacked = false;
    private float spinPower = 1.0f;

    private void OnCollisionEnter(Collision collision) {
        if (Smacked) {
            return;
        }

        if (collision.gameObject.tag == "Car" || collision.gameObject.tag == "Player") {
            // fly to space bruh
            Smacked = true;
            var rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;

            if (rb) {
                var v = collision.relativeVelocity;
                Vector3 rand;

                rand.x = Random.Range(-1.0f, 1.0f);
                rand.y = Random.Range(-1.0f, 1.0f);
                rand.z = Random.Range(-1.0f, 1.0f);

                rb.AddForce(v * (9.2f * spinPower) * rb.mass / 0.2f);
                rb.AddTorque(v + rand * 800.0f * rb.mass / 0.2f);
            }

            // Game over
            if (gameObject.CompareTag("Player")) {
                Audio.Instance.sfxSource.PlayOneShot(AudioClips.Instance.ouch);
                RCC.I.LoseGame();
            }
        }
    }

    private void Start() {
        spinPower = Random.Range(3.0f, 9.0f);
    }
}