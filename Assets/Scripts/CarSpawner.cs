using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject DeathCar;
    public GameObject CrowdSpawner;

    public void CarSpawn(int targetDepth)
    {
        if (RCC.I.track.targets.Count < targetDepth) //  not enough points
        {
            return;
        }

        Vector3[] targets = new Vector3[targetDepth];

        for (int i = 0; i < targetDepth; i++)
        {
            targets[i] = RCC.I.track.targets[targetDepth - i - 1] + new Vector3(0.0f, DeathCar.transform.localScale.y / 2.0f, 0.0f);

            float randX = Random.Range(-5.0f, 5.0f);
            float randZ = Random.Range(-5.0f, 5.0f);

            targets[i] += new Vector3(randX, 0.0f, randZ);
        }

        Instantiate(CrowdSpawner, RCC.I.track.targets[targetDepth - targetDepth / 4] + Vector3.up * 10.0f, Quaternion.identity);

        Debug.Log("post + " + RCC.I.track.targets.ToArray().Length);

        GameObject Car = Instantiate(DeathCar);
        Car.GetComponent<ObstacleManager>().PositionCar(RCC.I.track.targets[targetDepth - 1]);
        Car.GetComponent<ObstacleManager>().InitializeRC(targets);
    }
}
