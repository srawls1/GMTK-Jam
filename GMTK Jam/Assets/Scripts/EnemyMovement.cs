using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    StraightLine,
    StationaryCircle,
    MovingCircle,
    SineWave
}

[System.Serializable]
class MovementParameters
{
    public MovementType type;
    public float duration;
    public Transform startPosition;
    public Transform endPosition;
    public float amplitude;
    public float frequency;
    // -1 for permanent
    public int numberOfCircles;
}

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private MovementParameters[] behavior;
    [SerializeField] private bool loopMovement;
    [SerializeField] private bool deleteWhenFinished;

	IEnumerator Start()
    {
        yield return StartCoroutine(MovementRoutine());

        if (deleteWhenFinished)
        {
            Destroy(gameObject);
        }
	}

	IEnumerator MovementRoutine()
    {
        do {
            foreach (MovementParameters movement in behavior)
            {
                switch (movement.type)
                {
                    case MovementType.StraightLine:
                        yield return StartCoroutine(MoveInStraightLine(movement.startPosition.position,
                            movement.endPosition.position, movement.duration));
                        break;
                    case MovementType.StationaryCircle:
                        yield return StartCoroutine(MoveInStationaryCircle(movement.startPosition.position,
                            movement.amplitude, movement.frequency, movement.numberOfCircles));
                        break;
                    case MovementType.MovingCircle:
                        yield return StartCoroutine(MoveInMovingCircle(movement.startPosition.position,
                            movement.endPosition.position, movement.duration, movement.amplitude,
                            movement.frequency));
                        break;
                    case MovementType.SineWave:
                        yield return StartCoroutine(MoveInSineWave(movement.startPosition.position,
                            movement.endPosition.position, movement.duration, movement.amplitude,
                            movement.frequency));
                        break;
                }
            }
        } while (loopMovement);
    }

    IEnumerator MoveInStraightLine(Vector3 startPosition, Vector3 endPosition, float duration)
    {
        float timePassed = 0f;
        while (timePassed < 1f)
        {
            timePassed += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(startPosition, endPosition, timePassed);
            yield return null;
        }

        transform.position = endPosition;
    }

    IEnumerator MoveInStationaryCircle(Vector3 center, float radius, float frequency, int numberOfCircles)
    {
        while (numberOfCircles != 0)
        {
            float timePassed = 0f;
            while (timePassed < 1f)
            {
                timePassed += Time.deltaTime * frequency * 2 * Mathf.PI;
                transform.position = center + new Vector3(Mathf.Cos(timePassed),
                    Mathf.Sin(timePassed), 0f) * radius;
                yield return null;
            }
            --numberOfCircles;
        }
    }

    IEnumerator MoveInMovingCircle(Vector3 startingCenter, Vector3 endingCenter,
        float duration, float radius, float frequency)
    {
        float timePassed = 0f;
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float angle = timePassed * frequency * 2 * Mathf.PI;
            float centerAlpha = timePassed / duration;

            Vector3 center = Vector3.Lerp(startingCenter, endingCenter, centerAlpha);
            transform.position = center + new Vector3(Mathf.Cos(angle),
                Mathf.Sin(angle), 0f) * radius;
            yield return null;
        }
    }

    IEnumerator MoveInSineWave(Vector3 startPosition, Vector3 endPosition, float duration,
        float amplitude, float frequency)
    {
        float timePassed = 0f;
        Vector3 orthogonal = Vector3.Cross((endPosition - startPosition), Vector3.forward);
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            Vector3 lerped = Vector3.Lerp(startPosition, endPosition, timePassed / duration);
            float offset = Mathf.Sin(timePassed * frequency) * amplitude;
            transform.position = lerped + offset * orthogonal;
            yield return null;
        }
    }
}
