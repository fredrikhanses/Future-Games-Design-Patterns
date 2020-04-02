using System.Collections.Generic;
using UnityEngine;

[MonoSingletonConfigurationAttribute(resourcesPath:"EnemyManager")]
public class EnemyManager : MonoSingleton<EnemyManager>
{
    [SerializeField]
    private GameObject enemyPrefab;

    private EnemyController enemyController;
    private GameObject currentEnemy;

    /// <summary>
    ///    Creates an enemy at the origin.
    /// </summary>
    public void CreateEnemy()
    {
        Instantiate(enemyPrefab);
    }

    /// <summary>
    ///    Creates an enemy at a specific position.
    /// </summary>
    public void CreateEnemy(Vector3 spawnPosition)
    {
        currentEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    public void MoveEnemy(Vector3 targetPosition)
    {
        enemyController = currentEnemy.GetComponent<EnemyController>();
        enemyController.MoveTo(targetPosition);
    }

    public void Move(IEnumerable<Vector2Int> path, List<KeyValuePair<Vector2Int, Vector3>> mapPositions)
    {
        enemyController.Move(path, mapPositions);
    }
}
