using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotType
{
    StraightTowardPlayer,
    StraightInADirection,
    RadiallyOut
}

[System.Serializable]
class ShotParameters
{
    public GameObject bullet;
    public ShotType type;
    public float time;
    public int numberOfShots;
    public float centerAngle;
    public float angleRange;
}

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private List<ShotParameters> shots;

    private static Transform player;

    void Awake()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerMovement>().transform;
        }
    }

    IEnumerator Start()
    {
        shots.Sort((a, b) =>
        {
            if (a.time < b.time)
            {
                return -1;
            }
            if (a.time > b.time)
            {
                return 1;
            }
            return 0;
        });

        while (true) {
            float time = 0f;
            foreach (ShotParameters shot in shots)
            {
                yield return new WaitForSeconds(shot.time - time);
                time = shot.time;

                switch (shot.type)
                {
                    case ShotType.StraightTowardPlayer:
                        ShootTowardPlayer(shot.bullet);
                        break;
                    case ShotType.StraightInADirection:
                        ShootInDirection(shot.bullet, shot.centerAngle);
                        break;
                    case ShotType.RadiallyOut:
                        ShootRadiallyOut(shot.bullet, shot.centerAngle, shot.angleRange, shot.numberOfShots);
                        break;
                }
            }
        }
    }

    void ShootTowardPlayer(GameObject bullet)
    {
        Vector3 shotDir = player.position - transform.position;
        float angle = Mathf.Atan2(shotDir.y, shotDir.x) * Mathf.Rad2Deg;
        ShootInDirection(bullet, angle);
    }

    void ShootInDirection(GameObject bullet, float angle)
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        Instantiate(bullet, transform.position, rotation);
    }

    void ShootRadiallyOut(GameObject bullet, float centerAngle, float angleRange, int numberOfShots)
    {
        float angle = centerAngle - angleRange / 2;
        float angleStep = angleRange / (numberOfShots - 1);

        for (int i = 0; i <= numberOfShots; ++i)
        {
            ShootInDirection(bullet, angle);
            angle += angleStep;
        }
    }
}
