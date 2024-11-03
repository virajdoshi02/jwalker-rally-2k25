using UnityEngine;

public class AudioClips : MonoBehaviour
{
    public static AudioClips Instance { get; private set; }

    public AudioClip mainMenuMusic;
    public AudioClip inGameMusic;
    public AudioClip gameOverMusic;

    public AudioClip buttonClick;
    public AudioClip buttonHover;

    public AudioClip pauseOn;
    public AudioClip pauseOff;

    public AudioClip ready;
    public AudioClip go;

    public AudioClip gameOver;

    public AudioClip ouch;

    public AudioClip vroom;

    public AudioClip bowling;

    public AudioClip boop;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }
}