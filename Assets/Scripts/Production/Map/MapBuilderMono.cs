using AI;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class MapBuilderMono : MonoBehaviour
{
    [SerializeField] private Vector3 m_Origin = Vector3.zero;
    [SerializeField, Range(1, 5), Tooltip("Choose which map to generate")] private uint m_MapNumber = 4;
    [SerializeField, Range(0.1f, 1f)] private float m_MinSpawnInterval = 1f;
    [SerializeField, Range(1f, 5f)] private float m_MaxSpawnInterval = 3f;
    [SerializeField] private GameObjectScriptablePool m_PathTileScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_ObstacleTileScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_BombTowerScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_FreezeTowerScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_EnemyBaseScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_PlayerBaseScriptablePool;

    private bool m_UseBigEnemy = false;
    private int? m_CurrentEnemy = null;
    private float m_SpawnInterval = 1f;
    private readonly float m_TileDisplacement = 2.0f;
    private string m_MapName;
    private EnemyCounter m_EnemyCounter;
    private Camera m_MainCamera;
    private CameraMove m_CameraMove;
    private MapReaderMono m_MapReaderMono;
    private IPathFinder m_PathFinder;
    private IEnumerable<Vector2Int> m_Path;
    private MapData m_MapData = new MapData();
    private GameObjectScriptablePool m_CurrentScriptablePool;
    private readonly Dictionary<uint, string> m_Maps = new Dictionary<uint, string>
    {
        { 1, "map_1" },
        { 2, "map_2" },
        { 3, "map_3" },
        { 4, "map_4" },
        { 5, "map_5" }
    };

    private void Start()
    {
        m_EnemyCounter = FindObjectOfType<EnemyCounter>();
        m_MapReaderMono = GetComponent<MapReaderMono>();
        m_MainCamera = Camera.main;
        m_CameraMove = m_MainCamera.GetComponent<CameraMove>();
        GenerateMap();
        m_EnemyCounter.EnemyWaves = Mathf.RoundToInt(m_MapData.EnemyWaves.Count * 0.5f);
        m_EnemyCounter.StandardEnemies = m_MapData.EnemyWaves.Dequeue();
        m_EnemyCounter.BigEnemies = m_MapData.EnemyWaves.Dequeue();
        m_EnemyCounter.EnemyReinforcements = m_EnemyCounter.StandardEnemies + m_EnemyCounter.BigEnemies;
    }

    private void FixedUpdate()
    {
        if (m_MapData.MapLayout.Count > 0)
        {
            m_SpawnInterval -= Time.fixedDeltaTime;
            if (m_SpawnInterval <= 0f)
            {
                if (m_EnemyCounter.StandardEnemies > 0 && m_UseBigEnemy == false)
                {
                    m_CurrentEnemy = (int?)UnitType.Standard;
                    SpawnEnemy();
                    m_EnemyCounter.DecreaseStandardEnemies();
                    if (m_EnemyCounter.BigEnemies != 0)
                    {
                        m_UseBigEnemy = true;
                    }
                }
                else if (m_EnemyCounter.StandardEnemies == 0 && m_UseBigEnemy == false)
                {
                    m_UseBigEnemy = true;
                }
                else if (m_EnemyCounter.BigEnemies > 0 && m_UseBigEnemy)
                {
                    m_CurrentEnemy = (int?)UnitType.Big;
                    SpawnEnemy();
                    m_EnemyCounter.DecreaseBigEnemies(); ;
                    if (m_EnemyCounter.StandardEnemies != 0)
                    {
                        m_UseBigEnemy = false;
                    }
                }
                else if (m_EnemyCounter.BigEnemies == 0)
                {
                    m_UseBigEnemy = false;
                }
                if(m_EnemyCounter.StandardEnemies == 0 && m_EnemyCounter.BigEnemies == 0 && m_EnemyCounter.EnemyWaves >= 0)
                {
                    bool nextWave = false;
                    if (EnemyManager.Instance.ActiveEnemyControllers.Count <= 0)
                    {
                        nextWave = true;
                    }
                    if (nextWave)
                    {
                        m_EnemyCounter.DecreaseWaves();
                        if (m_MapData.EnemyWaves.Count > 1)
                        {
                            m_EnemyCounter.StandardEnemies = m_MapData.EnemyWaves.Dequeue();
                            m_EnemyCounter.BigEnemies = m_MapData.EnemyWaves.Dequeue();
                        }
                        m_UseBigEnemy = false;
                    }
                }
                else if (m_EnemyCounter.StandardEnemies == 0 && m_EnemyCounter.BigEnemies == 0 && m_EnemyCounter.EnemyWaves < 0)
                {
                    m_EnemyCounter.WinGame();
                }
                m_SpawnInterval = Random.Range(m_MinSpawnInterval, m_MaxSpawnInterval);
            }
        }
    }

    private void GenerateMap()
    {
        Setup();
        ReadMap();
        GeneratePath();
        CalculateWalkPoints(); 
        if (m_MapData.MapLayout != null && m_MapData.MapLayout.Count > 0)
        {
            foreach (KeyValuePair<Vector3, GameObject> objectPosition in m_MapData.MapLayout)
            {
                SelectScriptablePool(objectPosition.Value.tag);
                m_CurrentScriptablePool.Rent(true).transform.position = objectPosition.Key;
            }
        }
    }

    private void Setup()
    {
        m_MapReaderMono.Displacement = m_TileDisplacement;
        m_MapReaderMono.Origin = m_Origin;
        m_CameraMove.MoveCamera(m_Origin);
    }

    private void ReadMap()
    {
        m_MapName = m_Maps[m_MapNumber];
        m_MapData = m_MapReaderMono.ReadMap(m_MapName);
    }

    private void GeneratePath()
    {
        m_PathFinder = new Dijkstra(m_MapData.WalkableTiles);
        m_Path = m_PathFinder.FindPath(m_MapData.EnemySpawnTilePosition, m_MapData.PlayerBaseTilePosition);
    }

    private void CalculateWalkPoints()
    {
        foreach (Vector2Int tilePosition in m_Path)
        {
            foreach (KeyValuePair<Vector2Int, Vector3> mapPosition in m_MapData.MapPositions)
            {
                if (tilePosition.Equals(mapPosition.Key))
                {
                    m_MapData.Path.AddLast(mapPosition.Value);
                }
            }
        }
    }

    private void SelectScriptablePool(string tag)
    {
        if(m_PathTileScriptablePool.Prefab.CompareTag(tag))
        {
            m_CurrentScriptablePool = m_PathTileScriptablePool;
        }
        else if(m_ObstacleTileScriptablePool.Prefab.CompareTag(tag))
        {
            m_CurrentScriptablePool = m_ObstacleTileScriptablePool;
        }
        else if (m_FreezeTowerScriptablePool.Prefab.CompareTag(tag))
        {
            m_CurrentScriptablePool = m_FreezeTowerScriptablePool;
        }
        else if (m_BombTowerScriptablePool.Prefab.CompareTag(tag))
        {
            m_CurrentScriptablePool = m_BombTowerScriptablePool;
        }
        else if (m_EnemyBaseScriptablePool.Prefab.CompareTag(tag))
        {
            m_CurrentScriptablePool = m_EnemyBaseScriptablePool;
        }
        else
        {
            m_CurrentScriptablePool = m_PlayerBaseScriptablePool;
        }
    }

    private void SpawnEnemy()
    {
        EnemyManager.Instance.CreateEnemy(m_MapData.EnemySpawnWorldPosition, m_CurrentEnemy);
        EnemyManager.Instance.StartMoving(m_MapData.Path);
    }
}
