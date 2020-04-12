using System.Collections.Generic;
using UnityEngine;
using Tools;

[MonoSingletonConfigurationAttribute(resourcesPath:"EnemyManager")]
public class EnemyManager : MonoSingleton<EnemyManager>
{
    [SerializeField] private GameObjectScriptablePool m_EnemyScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_StrongEnemyScriptablePool;

    private GameState m_GameState;
    private GameObject m_CurrentEnemy;
    private EnemyController m_CurrentEnemyController;
    private GameObjectScriptablePool m_CurrentScriptablePool;
    private HashSet<EnemyController> m_EnemyControllers = new HashSet<EnemyController>();

    public HashSet<EnemyController> ActiveEnemyControllers { get => m_EnemyControllers; }

    private void Start()
    {
        m_GameState = FindObjectOfType<GameState>();
    }

    /// <summary> Creates an enemy at a specific position and then makes it start moving along a path.</summary>
    /// <param name="spawnPosition">Position to spawn the enemy at.</param>
    /// <param name="path">Path that the enemy should move along.</param>
    /// <param name="prefabIndex">Default null for random enemy, 0 for strong enemy, 1 for normal enemy.</param>
    public void CreateEnemy(Vector3 spawnPosition, int? prefabIndex = null)
    {
        SelectScriptablePool(prefabIndex);
        m_CurrentEnemy = m_CurrentScriptablePool.Rent(true);
        m_CurrentEnemyController = m_CurrentEnemy.GetComponent<EnemyController>();
        m_CurrentEnemyController.ResetPosition(spawnPosition);
        m_CurrentEnemyController.Reset();
        ActiveEnemyControllers.Add(m_CurrentEnemyController);
        m_GameState.IncreaseActiveEnemies();
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

    public void StartMoving(IEnumerable<Vector3> path)
    {
        m_CurrentEnemyController = m_CurrentEnemy.GetComponent<EnemyController>();
        m_CurrentEnemyController.StartMoving(path);
    }
}
