using UnityEngine;

public class RCC : MonoBehaviour
{
    public TrackGenerator track;
    public GameObject Player;

    public static RCC I { get; private set; }

    public CarSpawner carSpawner;
    private float distThreshold = 0.75f;

    public void CheckPlayerTargetDelta()
    {
        if (I.track.targets.Count == 0) return;

        Vector3 currentTarget = I.track.targets[0];
        Vector3 playerPos = Player.transform.position; // TO DO: replace with the actual player position
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