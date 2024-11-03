using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject DeathCar;

    public void CarSpawn()
    {
        if (RCC.I.track.targets.Count < 3) //  not enough points
        {
            return;
        }

        Vector3[] targets = new Vector3[3];
        Debug.Log(RCC.I.track.targets.ToArray());

        for (int i = 0; i < 3; i++)
        {
            targets[i] = RCC.I.track.targets[i];
            Debug.Log(targets[i]);
        }

        Debug.Log("post + " + RCC.I.track.targets.ToArray().Length);

        GameObject Car = Instantiate(DeathCar);
        Car.GetComponent<ObstacleManager>().PositionCar(RCC.I.track.targets[2]);
        Car.GetComponent<ObstacleManager>().InitializeRC(targets);
    }
}
