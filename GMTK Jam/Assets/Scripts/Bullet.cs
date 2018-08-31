using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private bool m_collectable;
    // TODO - We'll see about color shifting

    public bool collectable
    {
        get
        {
            return m_collectable;
        }
        // TODO - We'll see about changing this later
    }

	void Awake()
    {
		Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.rotation * Vector3.right * speed;
	}
}
