using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject sphere;

    // Start is called before the first frame update
    private void Start() {
        var pos = Vector3.zero;

        for (var z = 0; z < 10; z++) {
            Instantiate(sphere, pos, Quaternion.identity);
            pos.z += 10;
        }
    }

    // Update is called once per frame
    private void Update() { }
}