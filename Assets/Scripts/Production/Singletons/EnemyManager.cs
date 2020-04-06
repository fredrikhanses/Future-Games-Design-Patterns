using System.Collections.Generic;
using UnityEngine;

[MonoSingletonConfigurationAttribute(resourcesPath:"EnemyManager")]
public class EnemyManager : MonoSingleton<EnemyManager>
{
    [SerializeField] private GameObject enemyPrefab;

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

    public void MoveStart(LinkedList<Vector3> walkPoints)
    {
        enemyController = currentEnemy.GetComponent<EnemyController>();
        enemyController.MoveStart(walkPoints);
    }
}
