using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterRespawn : MonoBehaviour
{
    [SerializeField] private float respawnHealth;
    [SerializeField] private float respawnDelay;
    [SerializeField] private int numLives;
    [SerializeField] string GameOverScreen;

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
            SceneManager.LoadScene(GameOverScreen);
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
