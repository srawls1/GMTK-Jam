using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class EnemyActivation
{
    public float activateAfterTime;
    public GameObject enemy;
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<EnemyActivation> activations;
    [SerializeField] private int numRequiredToKill;

    int numKilled = 0;

	IEnumerator Start()
    {
		activations.Sort((a, b) =>
        {
            if (a.activateAfterTime < b.activateAfterTime)
            {
                return -1;
            }
            if (a.activateAfterTime > b.activateAfterTime)
            {
                return 1;
            }
            return 0;
        });

        float time = 0f;

        foreach (EnemyActivation activation in activations)
        {
            float waitTime = activation.activateAfterTime - time;
            if (waitTime > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }

            activation.enemy.SetActive(true);
            activation.enemy.GetComponent<Health>().OnDeath += () =>
            {
                RecordEnemyDead();
            };
            time = activation.activateAfterTime;
        }
	}

    void RecordEnemyDead()
    {
        ++numKilled;
        if (numKilled >= numRequiredToKill)
        {
            Debug.Log("WINNER!!!!!!!!!!!!");
        }
    }
}
