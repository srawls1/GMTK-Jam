using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour {
    public GameObject bullet;
    public float frequency;

	IEnumerator Start () {
		while (true)
        {
            Instantiate(bullet, transform.position, transform.rotation);
            yield return new WaitForSeconds(frequency);
        }
	}
}
