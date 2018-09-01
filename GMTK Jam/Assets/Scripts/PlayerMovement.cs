using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {


    public float HorizontalSpeed= 10;
    public float VerticalSpeed= 10;
    public float HorizontalAcceleration = 20;
    public float VerticalAcceleration = 20;
    public float HorizontalDeceleration = 40;
    public float VerticalDeceleration = 40;

    Rigidbody2D myRigidBody;

	// Use this for initialization
	void Start () {
        myRigidBody = gameObject.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {

        UpdateInput();

	}

    bool DecelerationRequired(float input, float velocity)
    {
        // Test if they have opposite signs, i.e. opposite directions
        if (input * velocity < 0)
        {
            return true;
        }

        return (Mathf.Abs(input) < Mathf.Abs(velocity));
    }

    void UpdateInput()
    {
        Vector2 velocity = myRigidBody.velocity;
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 acceleration = new Vector2(HorizontalAcceleration, VerticalAcceleration);
        acceleration = Vector2.Scale(input, acceleration);

        if (DecelerationRequired(input.x, velocity.x / HorizontalSpeed))
        {
            acceleration.x = -Mathf.Sign(velocity.x) * HorizontalDeceleration;
        }
        if (DecelerationRequired(input.y, velocity.y / VerticalSpeed))
        {
            acceleration.y = -Mathf.Sign(velocity.y) * VerticalDeceleration;
        }

        velocity += acceleration * Time.deltaTime;

        // Cap it at max speed
        if (velocity.x > HorizontalSpeed)
        {
            velocity.x = HorizontalSpeed;
        }
        if (velocity.y > VerticalSpeed)
        {
            velocity.y = VerticalSpeed;
        }

        // If input is zero and our velocity has crossed zero, no acceleration
        if (Mathf.Approximately(input.x, 0f) && velocity.x * myRigidBody.velocity.x < 0f)
        {
            velocity.x = 0f;
        }
        if (Mathf.Approximately(input.y, 0f) && velocity.y * myRigidBody.velocity.y < 0f)
        {
            velocity.y = 0f;
        }

        myRigidBody.velocity = velocity;

        // Vector2 newForce = new Vector2(0,0);
        // if (Input.GetKey(KeyCode.S))
        // {
        //    newForce.y = -VerticalSpeed;
        //
        // }
        // if (Input.GetKey(KeyCode.W))
        // {
        //     newForce.y = VerticalSpeed;
        //
        // }
        // if (Input.GetKey(KeyCode.A))
        // {
        //     newForce.x = -HorizontalSpeed;
        //
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     newForce.x = HorizontalSpeed;
        //
        // }
        // myRigidBody.AddForce(newForce);
    }
}
