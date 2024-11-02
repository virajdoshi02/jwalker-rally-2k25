using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject RallyCar;
    public GameObject[] Targets;

    RallyController RC_Controller;

    void Start()
    {
        List<Vector3> posTargets = new List<Vector3>();
        foreach (GameObject target in Targets)
        {
            posTargets.Add(target.transform.position);
        }

        RC_Controller = new RallyController(RallyCar, posTargets.ToArray(), false);
    }

    // Update is called once per frame
    void Update()
    {
        RC_Controller.LaunchCar();
    }
}

public class RallyController
{
    GameObject RallyCar;
    Rigidbody CarRB;
    Vector3[] Targets;
    int CurrentTargetIdx = 0;
    float AdditionalThrust = 4.0f;
    float SteeringPower = 1.0f;

    public RallyController(GameObject car, Vector3[] Targets, bool DynamicParams)
    {
        RallyCar = car;
        CarRB = RallyCar.GetComponent<Rigidbody>();
        this.Targets = Targets;

        // additional thrust 
        if (DynamicParams)
        {
            AdditionalThrust = Random.Range(4.0f, 8.0f);
            SteeringPower = Random.Range(4.0f, 8.0f);
        }

        LaunchCar();
    }

    float DistanceToTarget(Vector3 target)
    {
        return (target - RallyCar.transform.position).magnitude;
    }

    public void LaunchCar()
    {
        if (CurrentTargetIdx >= Targets.Length)
        {
            return;
        }

        Vector3 carToBegin = (Targets[CurrentTargetIdx] - CarRB.position);
        Vector3 launchDir = Vector3.Normalize(carToBegin);
        float BeginDist = carToBegin.magnitude;

        Debug.DrawRay(CarRB.position, launchDir);

        if (BeginDist < 2.0f)
        {
            CurrentTargetIdx++;
        }

        Vector3 forward = RallyCar.transform.forward;
        Vector3 cross = Vector3.Cross(forward, launchDir);
        float angleToTarget = Vector3.SignedAngle(forward, launchDir, Vector3.up);

        CarRB.AddTorque(cross * angleToTarget * SteeringPower);
        CarRB.AddForce(RallyCar.transform.forward * AdditionalThrust);
    }
}
