﻿using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{

    [System.Serializable]
    public class MoveSettings
    {
        public float forwardVel = 12;
        public float rotateVel = 100;
        public float jumpVel = 25;
        public float distToGround = 0.6f;
        public LayerMask ground;
    }

    [System.Serializable]
    public class PhysSettings
    {
        public float downAccel = 0.75f;
    }

    [System.Serializable]
    public class InputSettings
    {
        public float inputDelay = 0.1f;
        public string FORWARD_AXIS = "Vertical";
        public string TURN_AXIS = "Horizontal";
        public string JUMP_AXIS = "Jump";
    }

    public MoveSettings moveSettings = new MoveSettings();
    public PhysSettings physSettings = new PhysSettings();
    public InputSettings inputSettings = new InputSettings();

    Vector3 velocity = Vector3.zero;
    Quaternion targetRotation;
    private Rigidbody rBody;
    float forwardInput, turnInput, jumpInput;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    bool Grounded()
    {
        
        return Physics.Raycast(transform.position, Vector3.down, moveSettings.distToGround, moveSettings.ground);
    }

    void GetInput()
    {
        forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS);
        turnInput = Input.GetAxis(inputSettings.TURN_AXIS);
        jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS);
    }

    void Turn()
    {
        if (Mathf.Abs(turnInput) > inputSettings.inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(moveSettings.rotateVel * turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
    }

    void Run()
    {
        if (Mathf.Abs(forwardInput) > inputSettings.inputDelay)
        {
            // move
            velocity.z = forwardInput * moveSettings.forwardVel * Time.deltaTime * 100;
        }
        else
            // zero velocity
            velocity.z = 0;
    }

    // Use this for initialization
    void Start()
    {
        targetRotation = transform.rotation;
        rBody = GetComponent<Rigidbody>();

        forwardInput = turnInput = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Turn();
    }

    void FixedUpdate()
    {
        //if (Grounded()) Debug.Log("YEEEEEEEEEA");
        Run();
        Jump();

        rBody.velocity = transform.TransformDirection(velocity);
    }

    void Jump()
    {
        if (jumpInput > 0 && Grounded())
        {
            //Debug.Log("YEEEEEEEEEA");
            velocity.y = moveSettings.jumpVel;
        }
        else if (jumpInput == 0 && Grounded())
        {
            // zero velocity
            velocity.y = 0;
        }
        else
        {
            velocity.y -= physSettings.downAccel;
        }
    }
}