using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    private Vector3 targetPosition;
    private bool move;
    private float resetTime = 3.0f;
    private float currentTime = 0.0f;

    public void MoveTo(Vector3 targetPosition)
    {
        move = true;
        this.targetPosition = targetPosition;
    }

    public void Move(IEnumerable<Vector2Int> path, List<KeyValuePair<Vector2Int, Vector3>> mapPositions)
    {
        foreach (Vector2Int tilePosition in path)
        {
            foreach(KeyValuePair<Vector2Int, Vector3> mapPosition in mapPositions)
            {
                if (tilePosition.Equals(mapPosition.Key))
                {
                    MoveTo(mapPosition.Value);
                }
            }
            
        }
    }

    private void FixedUpdate()
    {
        if(move)
        {
            currentTime += Time.fixedDeltaTime;
            if (currentTime >= resetTime)
            {
                currentTime = 0.0f;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);
                if (transform.position.Equals(targetPosition))
                {
                    move = false;
                }
            }
        }
    }
}
