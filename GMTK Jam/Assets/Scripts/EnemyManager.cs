using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
class EnemyActivation
{
    public float activateAfterTime;
    public GameObject enemy;
    public bool requiredToKill;
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<EnemyActivation> activations;
    [SerializeField] private UnityEvent onEnemiesCleared;

    int numRequiredToKill;
    int numKilled = 0;

    void Awake()
    {
        numRequiredToKill = 0;
        foreach (EnemyActivation activation in activations)
        {
            if (activation.requiredToKill)
            {
                ++numRequiredToKill;
            }
        }
    }

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

        for (int i = 0; i < activations.Count; ++i)
        {
            int index = i;
            EnemyActivation activation = activations[index];

            float waitTime = activation.activateAfterTime - time;
            if (waitTime > 0)
            {
                yield return new WaitForSeconds(waitTime);
            }

            activation.enemy.SetActive(true);
            activation.enemy.GetComponent<Health>().OnDeath += () =>
            {
                RecordEnemyDead(index);
            };
            time = activation.activateAfterTime;
        }
	}

    void RecordEnemyDead(int index)
    {
        if (activations[index].requiredToKill)
        {
            ++numKilled;
        }

        if (numKilled == numRequiredToKill)
        {
            onEnemiesCleared.Invoke();
        }
    }
}
