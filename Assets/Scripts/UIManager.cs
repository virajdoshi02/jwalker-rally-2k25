using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text distanceText;
    [SerializeField]
    private TMP_Text timeText;

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

    private void Update() {
        if (!timerPaused) timer += Time.deltaTime;

        var ts = new TimeSpan((long)(timer * 10_000_000));
        timeText.text = ts.ToString(@"mm\:ss\.fff");
    }
}