using System.Collections.Generic;
using UnityEngine;
using Tools;

[MonoSingletonConfigurationAttribute(resourcesPath:"EnemyManager")]
public class EnemyManager : MonoSingleton<EnemyManager>
{
    [SerializeField] private GameObjectScriptablePool m_StandardEnemyScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_BigEnemyScriptablePool;
    [SerializeField, Range(1, 99)] private int k_BigEnemyChance = 3;

    private EnemyCounter m_EnemyCounter;
    private GameObject m_CurrentEnemy;
    private EnemyController m_CurrentEnemyController;
    private GameObjectScriptablePool m_CurrentScriptablePool;
    private HashSet<EnemyController> m_EnemyControllers = new HashSet<EnemyController>();

    public HashSet<EnemyController> ActiveEnemyControllers { get => m_EnemyControllers; }

    private void Start()
    {
        m_EnemyCounter = FindObjectOfType<EnemyCounter>();
    }

    /// <summary> Creates an enemy at a specific position.</summary>
    /// <param name="spawnPosition">Position to spawn the enemy at.</param>
    /// <param name="prefabIndex">Default null for random enemy, 0 for normal enemy, 1 for strong enemy.</param>
    public void CreateEnemy(Vector3 spawnPosition, int? prefabIndex = null)
    {
        SelectScriptablePool(prefabIndex);
        m_CurrentEnemy = m_CurrentScriptablePool.Rent(true);
        m_CurrentEnemyController = m_CurrentEnemy.GetComponent<EnemyController>();
        m_CurrentEnemyController.ResetPosition(spawnPosition);
        m_CurrentEnemyController.Reset();
        ActiveEnemyControllers.Add(m_CurrentEnemyController);
        m_EnemyCounter.IncreaseActiveEnemies();
    }

    private void SelectScriptablePool(int? prefabIndex)
    {
        int bigChance = 100 - k_BigEnemyChance;
        if (prefabIndex == null)
        {
            System.Random random = new System.Random();
            prefabIndex = random.Next(0, bigChance);
        }
        if (prefabIndex == (int?)UnitType.Big)
        {
            m_CurrentScriptablePool = m_BigEnemyScriptablePool;
        }
        else if (prefabIndex == (int?)UnitType.Standard)
        {
            m_CurrentScriptablePool = m_StandardEnemyScriptablePool;
        }
        else
        {
            m_CurrentScriptablePool = m_StandardEnemyScriptablePool;
        }
    }

    /// <summary> Makes the current enemy move along a path.</summary>
    /// <param name="path">Path that the enemy should move along.</param>
    public void StartMoving(IEnumerable<Vector3> path)
    {
        m_CurrentEnemyController = m_CurrentEnemy.GetComponent<EnemyController>();
        m_CurrentEnemyController.StartMoving(path);
    }
}
