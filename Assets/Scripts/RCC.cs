using UnityEngine;

public class RCC : MonoBehaviour
{
    public TrackGenerator track;

    public static RCC I { get; private set; }

    private CarSpawner carSpawner;
    private float distThreshold;

    public void CheckPlayerTargetDelta()
    {
        Vector3 currentTarget = I.track.targets[0];
        Vector3 playerPos = Vector3.zero; // TO DO: replace with the actual player position
        float distToTarget = (playerPos - currentTarget).magnitude;

        if (distToTarget < distThreshold)
        {
            carSpawner.CarSpawn();
            I.track.targets.RemoveAt(0); // pop
            track.IncrementTrack();
        }
    }

    private void Awake() {
        if (I == null) {
            I = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        CheckPlayerTargetDelta();
    }
}