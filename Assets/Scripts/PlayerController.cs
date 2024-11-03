using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveKey
{ 
    UP,
    LEFT,
    RIGHT,
    DOWN
}

public class PlayerController : MonoBehaviour
{

    RigidBody rb;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<RigidBody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("W"))
        {
            rb.addForce(cam.forward);
        }
        else if (Input.)
    }
}

public class PlayerMovement : MonoBehaviour
{
    PlayerController player;

    PlayerMovement(PlayerController player)
    {
        this.player = player;
    }

    public void MovePlayer(MoveKey input)
    {
        if (input == MoveKey.UP)
        {
            
        }
        if (input == MoveKey.LEFT)
        {

        }
        if (input == MoveKey.RIGHT)
        {

        }
        if (input == MoveKey.DOWN)
        {

        }
    }
}
