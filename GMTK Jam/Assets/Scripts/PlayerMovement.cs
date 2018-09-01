using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    
    public float HorizontalSpeed= 10;

    public float VerticalSpeed= 10;

    Rigidbody2D myRigidBody;

	// Use this for initialization
	void Start () {
        myRigidBody = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        UpdateInput();
		
	}

    void UpdateInput()
    {
        Vector2 newForce = new Vector2(0,0);
        if (Input.GetKey(KeyCode.S))
        {
           newForce.y = -VerticalSpeed;
        
        }
        if (Input.GetKey(KeyCode.W))
        {
            newForce.y = VerticalSpeed;

        }
        if (Input.GetKey(KeyCode.A))
        {
            newForce.x = -HorizontalSpeed;

        }
        if (Input.GetKey(KeyCode.D))
        {
            newForce.x = HorizontalSpeed;

        }
        myRigidBody.AddForce(newForce);
        print(newForce);
    }
}
