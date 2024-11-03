using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RCC : MonoBehaviour
{
    public TrackGenerator track;
    public GameObject Player;

    [SerializeField]
    private UIManager ui;

    public static RCC I { get; private set; }

    public CarSpawner carSpawner;
    private float distThreshold = 10.0f;

    private bool start = false;
    private int terribleHack = 1;
    private bool lost;

    public void CheckPlayerTargetDelta() {
        if (Player == null) return;

        if (I.track.targets.Count == 0) return;
        if (!start) Player.transform.position = I.track.targets[0] + Vector3.up * 2.0f;
        if (!start) Audio.Instance.musicSource.PlayOneShot(AudioClips.Instance.inGameMusic);
        start = true;

        var currentTarget = I.track.targets[2];
        var playerPos = Player.transform.position; // TO DO: replace with the actual player position
        var distToTarget = (playerPos - currentTarget).magnitude;

        if (distToTarget < distThreshold) {
            carSpawner.CarSpawn(5);
            //carSpawner.CarSpawn(7);
            carSpawner.CarSpawn(9);

            track.IncrementTrack();
            I.track.targets.RemoveAt(0);

            // Increment score!
            ui.IncreaseDistance();

            Audio.Instance.sfxSource.PlayOneShot(AudioClips.Instance.boop);

            if (terribleHack == 0) {
                // Debug.Log("bye son");
                I.track.PopPlane();
            }

            terribleHack = (terribleHack + 1) % 3;
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

    private void Update() {
        CheckPlayerTargetDelta();

        if (lost) return;
        if (Player.transform.position.y < -10) {
            Player.GetComponent<PlayerController>().cursorLocked = false;
            Player.GetComponent<PlayerController>().isPlaying = false;
            LoseGame();
        }
    }

    public void LoseGame() {
        if (lost) return;
        lost = true;

        Audio.Instance.sfxSource.PlayOneShot(AudioClips.Instance.ouch);

        ui.PauseTimer();

        ui.deathDistance.text = ui.distanceText.text;
        ui.deathTime.text = ui.timeText.text;

        ui.deathCanvas.gameObject.SetActive(true);
        ui.infoCanvas.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(ui.FadeRedDeath());
        StartCoroutine(ui.AnimOuch());
    }
}