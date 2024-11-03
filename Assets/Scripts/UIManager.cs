using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text distanceText;
    public TMP_Text timeText;

    public Canvas deathCanvas;
    public Canvas infoCanvas;
    public CanvasGroup redCanvasGroup;
    public Image ouch;
    public TMP_Text deathDistance;
    public TMP_Text deathTime;
    public CanvasGroup btnRestart;
    public CanvasGroup btnMenu;

    private int distance;
    private Coroutine animDistance;

    private float timer;
    private bool timerPaused;

    private IEnumerator AnimateDistanceUp() {
        var length = 10f;
        var elapsed = 0f;

        while (elapsed <= length) {
            var num = (int)Mathf.Floor(elapsed);
            var newDist = distance + num;
            distanceText.text = $"{newDist}m";

            // 10 seconds / 25 = 0.4s total anim time
            elapsed += Time.deltaTime * 25f;
            yield return null;
        }

        distance += 10;
        distanceText.text = $"{distance}m";
    }

    public void IncreaseDistance() {
        if (animDistance != null) {
            StopCoroutine(animDistance);
        }

        animDistance = StartCoroutine(AnimateDistanceUp());
    }

    public void PauseTimer() {
        timerPaused = true;
    }

    public IEnumerator FadeRedDeath() {
        var length = 1f;
        var elapsed = 0f;

        while (elapsed <= length) {
            redCanvasGroup.alpha = 1f - elapsed;

            elapsed += Time.deltaTime;
            yield return null;
        }

        redCanvasGroup.alpha = 0f;
    }

    public IEnumerator AnimOuch() {
        var length = 0.4f;
        var elapsed = 0f;
        var blue = true;

        var count = 0;

        var blueCol = new Color(0.09803922f, 0.1137255f, 1f);

        yield return new WaitForSeconds(1.25f);

        deathDistance.color = Color.white;
        deathTime.color = Color.white;

        btnRestart.alpha = 1;
        btnRestart.interactable = true;
        btnMenu.alpha = 1;
        btnMenu.interactable = true;

        while (elapsed <= length) {
            ouch.color = blue ? blueCol : Color.white;

            if (count > 1) {
                blue = !blue;
                count = 0;
            }
            else {
                count++;
            }


            elapsed += Time.deltaTime;
            yield return null;
        }

        ouch.color = blueCol;

        length = 2f;
        elapsed = 0f;

        while (elapsed <= length) {
            ouch.color = Color.Lerp(blueCol, Color.white, elapsed / 2f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        ouch.color = Color.white;
    }

    private void Update() {
        if (timerPaused) return;

        timer += Time.deltaTime;
        var ts = new TimeSpan((long)(timer * 10_000_000));
        timeText.text = ts.ToString(@"mm\:ss\.fff");
    }
}