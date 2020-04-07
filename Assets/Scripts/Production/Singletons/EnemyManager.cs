using System.Collections.Generic;
using UnityEngine;
using Tools;

[MonoSingletonConfigurationAttribute(resourcesPath:"EnemyManager")]
public class EnemyManager : MonoSingleton<EnemyManager>
{
    [SerializeField] private GameObjectScriptablePool m_StrongEnemyScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_EnemyScriptablePool;

    private EnemyController m_CurrentEnemyController;
    private GameObject m_CurrentEnemy;
    private GameObjectScriptablePool m_CurrentScriptablePool;

    /// <summary>
    ///    Creates an enemy at a specific position.
    /// </summary>
    public void CreateEnemy(Vector3 spawnPosition, LinkedList<Vector3> walkPoints)
    {
        SelectEnemy();
        m_CurrentEnemy = m_CurrentScriptablePool.Rent(true);
        m_CurrentEnemyController = m_CurrentEnemy.GetComponent<EnemyController>();
        m_CurrentEnemyController.Reset(spawnPosition);
        m_CurrentEnemyController.transform.position = spawnPosition;
        MoveStart(walkPoints);
    }

    private void SelectEnemy()
    {
        System.Random random = new System.Random();
        int prefabIndex = random.Next(0, 3);
        if (prefabIndex == 0)
        {
            m_CurrentScriptablePool = m_StrongEnemyScriptablePool;
        }
        else
        {
            m_CurrentScriptablePool = m_EnemyScriptablePool;
        }
    }

    private void MoveStart(LinkedList<Vector3> walkPoints)
    {
        m_CurrentEnemyController = m_CurrentEnemy.GetComponent<EnemyController>();
        m_CurrentEnemyController.MoveStart(walkPoints);
    }
}
