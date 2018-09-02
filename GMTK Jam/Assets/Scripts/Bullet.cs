using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private bool m_collectable;
    [SerializeField] private float m_damage;
    [SerializeField] private int m_charge;
    [SerializeField] private Color safeColor;
    [SerializeField] private Color dangerousColor;
    [SerializeField] private float colorSmoothing;

    private bool currentlyDangerous;
    private SpriteRenderer renderer;

    private static CharacterShooting character;

    public bool collectable
    {
        get
        {
            return m_collectable && !currentlyDangerous;
        }
    }

    public float damage
    {
        get
        {
            return m_damage;
        }
    }

    public int charge
    {
        get
        {
            return m_charge;
        }
    }

	void Awake()
    {
		Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        rigidbody.velocity = transform.rotation * Vector3.right * speed;
        currentlyDangerous = !collectable;
	}

    void Start()
    {
        if (character == null)
        {
            character = FindObjectOfType<CharacterShooting>();
        }
    }

    void Update()
    {
        if (character != null)
        {
            if (character.charging)
            {
                currentlyDangerous = true;
            }
            else if (m_collectable)
            {
                currentlyDangerous = false;
            }
        }
        Color goalColor = collectable ? safeColor : dangerousColor;
        renderer.color = Color.Lerp(renderer.color, goalColor, colorSmoothing * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
