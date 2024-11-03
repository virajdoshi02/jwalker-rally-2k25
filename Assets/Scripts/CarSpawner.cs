using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject DeathCar;
    public GameObject CrowdSpawner;

    public void CarSpawn(int targetDepth) {
        if (RCC.I.track.targets.Count < targetDepth) //  not enough points
        {
            return;
        }

        var targets = new Vector3[targetDepth];

        for (var i = 0; i < targetDepth; i++) {
            targets[i] = RCC.I.track.targets[targetDepth - i - 1] +
                         new Vector3(0.0f, DeathCar.transform.localScale.y, 0.0f);

            var randX = Random.Range(-1.0f, 1.0f);
            var randZ = Random.Range(-1.0f, 1.0f);
            var randRange = 7.5f;

            targets[i] += new Vector3(randX * randRange, 0.0f, randZ * randRange);
        }

        var crowdSpwna = Instantiate(CrowdSpawner,
                                     RCC.I.track.targets[targetDepth - targetDepth / 4] + Vector3.up * 10.0f,
                                     Quaternion.identity);
        Destroy(crowdSpwna, 10.0f);

        // Debug.Log("post + " + RCC.I.track.targets.ToArray().Length);

        var Car = Instantiate(DeathCar);
        Car.GetComponent<ObstacleManager>().PositionCar(RCC.I.track.targets[targetDepth - 1]);
        Car.GetComponent<ObstacleManager>().InitializeRC(targets);
    }
}