using UnityEngine;

public class RCC : MonoBehaviour
{
    public TrackGenerator track;
    public GameObject Player;

    public static RCC I { get; private set; }

    public CarSpawner carSpawner;
    private float distThreshold = 10.0f;

    bool start = false;

    public void CheckPlayerTargetDelta()
    {
        if (I.track.targets.Count == 0) return;
        if (!start) Player.transform.position = I.track.targets[0] + Vector3.up * 2.0f;
        start = true;

        Vector3 currentTarget = I.track.targets[0];
        Vector3 playerPos = Player.transform.position; // TO DO: replace with the actual player position
        float distToTarget = (playerPos - currentTarget).magnitude;

        if (distToTarget < distThreshold)
        {
            carSpawner.CarSpawn(5);
            track.IncrementTrack();
            I.track.targets.RemoveAt(0); // pop
        }
    }

    private void Awake() {
        if (I == null) {
            I = this;
            //carSpawner = new CarSpawner();
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