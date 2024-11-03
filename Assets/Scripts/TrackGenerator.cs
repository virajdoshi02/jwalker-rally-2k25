using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject sphere;
    public List<Vector3> targets;
    
    public void IncrementTrack()
    {
        Vector3 lastPos = transform.position;

        if (targets.Count > 0) lastPos = targets[targets.Count - 1];
        Vector3 nextPos = lastPos - transform.right * 10.0f;

        targets.Add(nextPos);
        Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), nextPos, Quaternion.identity);
    }

    // Start is called before the first frame update
    private void Start() {
        Debug.Log("pee");

        for (var z = 0; z < 3; z++) {
            IncrementTrack();
        }
    }

    // Update is called once per frame
    private void Update() { }
}