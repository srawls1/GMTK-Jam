using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRespawn : MonoBehaviour
{
    [SerializeField] private float respawnHealth;
    [SerializeField] private float respawnDelay;
    [SerializeField] private int numLives;

    private Health health;
    private Rigidbody2D rigidBody;
    private Vector3 startingPosition;

	void Start()
    {
        startingPosition = transform.position;
		health = GetComponent<Health>();
        health.OnDeath += () => Invoke("Respawn", respawnDelay);
        rigidBody = GetComponent<Rigidbody2D>();
	}

	void Respawn()
    {
        if (numLives == 0)
        {
            Debug.Log("LOSER!!!!!!!!!");
        }
        else
        {
            --numLives;
            transform.position = startingPosition;
            rigidBody.velocity = Vector2.zero;
            health.Heal(respawnHealth);
        }
    }
}
