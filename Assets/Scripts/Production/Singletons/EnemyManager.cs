using System.Collections.Generic;
using UnityEngine;
using Tools;

[MonoSingletonConfigurationAttribute(resourcesPath:"EnemyManager")]
public class EnemyManager : MonoSingleton<EnemyManager>
{
    [SerializeField] private GameObjectScriptablePool m_EnemyScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_StrongEnemyScriptablePool;
    
    private GameObject m_CurrentEnemy;
    private EnemyController m_CurrentEnemyController;
    private GameObjectScriptablePool m_CurrentScriptablePool;
    private HashSet<EnemyController> m_EnemyControllers = new HashSet<EnemyController>();

    public HashSet<EnemyController> EnemyControllers { get => m_EnemyControllers; }

    /// <summary> Creates an enemy at a specific position and then makes it start moving along a path.</summary>
    /// <param name="spawnPosition">Position to spawn the enemy at.</param>
    /// <param name="path">Path that the enemy should move along.</param>
    /// <param name="prefabIndex">Default null for random enemy, 0 for strong enemy, 1 for normal enemy.</param>
    public void CreateEnemy(Vector3 spawnPosition, IEnumerable<Vector3> path, int? prefabIndex = null)
    {
        SelectScriptablePool(prefabIndex);
        m_CurrentEnemy = m_CurrentScriptablePool.Rent(true);
        m_CurrentEnemyController = m_CurrentEnemy.GetComponent<EnemyController>();
        m_CurrentEnemyController.Reset(spawnPosition);
        if (EnemyControllers.Contains(m_CurrentEnemyController) == false)
        {
            EnemyControllers.Add(m_CurrentEnemyController);
        }
        MoveStart(path);
    }

    private void SelectScriptablePool(int? prefabIndex)
    {
        if (prefabIndex == null)
        {
            System.Random random = new System.Random();
            prefabIndex = random.Next(0, 3);
        }
        if (prefabIndex == 0)
        {
            m_CurrentScriptablePool = m_StrongEnemyScriptablePool;
        }
        else
        {
            m_CurrentScriptablePool = m_EnemyScriptablePool;
        }
    }

    private void MoveStart(IEnumerable<Vector3> path)
    {
        m_CurrentEnemyController = m_CurrentEnemy.GetComponent<EnemyController>();
        m_CurrentEnemyController.MoveStart(path);
    }
}
