using UnityEngine;

public class RCC : MonoBehaviour
{
    public TrackGenerator track;
    public GameObject Player;

    public static RCC I { get; private set; }

    private CarSpawner carSpawner;
    private float distThreshold;

    public void CheckPlayerTargetDelta()
    {
        if (I.track.targets.Count == 0) return;

        Vector3 currentTarget = I.track.targets[0];
        Vector3 playerPos = Player.transform.position; // TO DO: replace with the actual player position
        float distToTarget = (playerPos - currentTarget).magnitude;

        if (distToTarget < distThreshold)
        {
            Debug.Log("Go on to the next one.");
            carSpawner.CarSpawn();
            I.track.targets.RemoveAt(0); // pop
            track.IncrementTrack();
        }
    }

    public void JunkTempFunctionThatYouShouldRemove()
    {
        Player.transform.position += transform.right * Time.deltaTime * -3.0f;
    }

    private void Awake() {
        if (I == null) {
            I = this;
            carSpawner = new CarSpawner();
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        CheckPlayerTargetDelta();
        JunkTempFunctionThatYouShouldRemove();
    }
}