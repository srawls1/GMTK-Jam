using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private bool m_collectable;
    [SerializeField] private float m_damage;
    // TODO We'll see about color shifting

    public bool collectable
    {
        get
        {
            return m_collectable;
        }
        // TODO We'll see about setting this later
    }

    public float damage
    {
        get
        {
            return m_damage;
        }
    }

	void Awake()
    {
		Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.rotation * Vector3.right * speed;
	}
}
