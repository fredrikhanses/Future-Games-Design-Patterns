﻿using AI;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class MapBuilderMono : MonoBehaviour
{
    [SerializeField] private Vector3 m_Origin = Vector3.zero;
    [SerializeField, Range(1, 5), Tooltip("Choose which map to generate")] private uint m_MapNumber = 4;
    [SerializeField, Range(0.1f, 1f)] private float m_MinSpawnInterval = 1f;
    [SerializeField, Range(1f, 5f)] private float m_MaxSpawnInterval = 3f;
    [SerializeField, Tooltip("Only works in Play Mode")] private bool m_GenerateMap;
    [SerializeField, Tooltip("Only works in Play Mode")] private bool m_PlayMap;
    [SerializeField, Tooltip("Only works in Play Mode")] private bool m_ClearMap;
    [SerializeField, Tooltip("Only works in Play Mode")] private bool m_PauseGame;
    [SerializeField] private GameObjectScriptablePool m_PathTileScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_ObstacleTileScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_BombTowerScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_FreezeTowerScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_EnemyBaseScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_PlayerBaseScriptablePool;

    private bool m_UseStrongEnemy = false;
    //private int m_NormalEnemies;
    //private int m_StrongEnemies;
    private int? m_CurrentEnemy = null;
    private float m_SpawnInterval = 1f;
    private float m_TileDisplacement = 2.0f;
    private float m_OriginalTimeScale;
    private string m_MapName;
    private Player m_Player;
    private GameState m_GameState;
    private Camera m_MainCamera;
    private MoveCamera m_MoveCamera;
    private MapReaderMono m_MapReaderMono;
    private IPathFinder m_PathFinder;
    private IEnumerable<Vector2Int> m_Path;
    private MapData m_MapData = new MapData();
    private GameObjectScriptablePool m_CurrentScriptablePool;
    private Dictionary<uint, string> m_Maps = new Dictionary<uint, string>
    {
        { 1, "map_1" },
        { 2, "map_2" },
        { 3, "map_3" },
        { 4, "map_4" },
        { 5, "map_5" }
    };
    private const string k_DontDestroy = "DontDestroy";
    private const string k_ScriptablePool = "ScriptablePool";

    private void Awake()
    {
        m_OriginalTimeScale = Time.timeScale;
    }

    private void Start()
    {
        m_Player = FindObjectOfType<Player>();
        m_GameState = FindObjectOfType<GameState>();
        m_MapReaderMono = GetComponent<MapReaderMono>();
        m_MainCamera = Camera.main;
        m_MoveCamera = m_MainCamera.GetComponent<MoveCamera>();
        GenerateMap();
        m_GameState.NormalEnemies = m_MapData.EnemyWaves.Dequeue();
        m_GameState.StrongEnemies = m_MapData.EnemyWaves.Dequeue();
        m_GameState.WaveNumber = m_MapData.EnemyWaves.Count - 1;
        
    }

    private void OnValidate()
    {
        if (m_GenerateMap && Application.isPlaying && m_MapData.MapLayout.Count <= 0)
        {
            GenerateMap();
            m_GenerateMap = false;
        }
        else
        {
            m_GenerateMap = false;
        }
        if (m_ClearMap && Application.isPlaying && m_MapData.MapLayout.Count > 0)
        {
            Time.timeScale = m_OriginalTimeScale;
            ClearMap();
            m_ClearMap = false;
        }
        else
        {
            m_ClearMap = false;
        }
        if (m_PlayMap && Application.isPlaying && m_MapData.MapLayout.Count > 0)
        {
            PlayMap();
            m_PlayMap = false;
        }
        else
        {
            m_PlayMap = false;
        }
        if (m_PauseGame && Application.isPlaying && m_MapData.MapLayout.Count > 0)
        {
            Time.timeScale = 0f;
        }
        else if (!m_PauseGame && Application.isPlaying && m_MapData.MapLayout.Count > 0)
        {
            Time.timeScale = m_OriginalTimeScale;
        }
        else if (m_PauseGame)
        {
            m_PauseGame = false;
        }
    }

    private void FixedUpdate()
    {
        if (m_MapData.MapLayout.Count > 0)
        {
            m_SpawnInterval -= Time.fixedDeltaTime;
            if (m_SpawnInterval <= 0f)
            {
                if (m_GameState.NormalEnemies > 0 && m_UseStrongEnemy == false)
                {
                    m_CurrentEnemy = 1;
                    PlayMap();
                    m_GameState.DecreaseNormalEnemies();
                    if (m_GameState.StrongEnemies != 0)
                    {
                        m_UseStrongEnemy = true;
                    }
                }
                else if (m_GameState.NormalEnemies == 0 && m_UseStrongEnemy == false)
                {
                    m_UseStrongEnemy = true;
                }
                else if (m_GameState.StrongEnemies > 0 && m_UseStrongEnemy)
                {
                    m_CurrentEnemy = 0;
                    PlayMap();
                    m_GameState.DecreaseStrongEnemies(); ;
                    if (m_GameState.NormalEnemies != 0)
                    {
                        m_UseStrongEnemy = false;
                    }
                }
                else if (m_GameState.StrongEnemies == 0)
                {
                    m_UseStrongEnemy = false;
                }
                if(m_GameState.NormalEnemies == 0 && m_GameState.StrongEnemies == 0 && m_GameState.WaveNumber >= 0)
                {
                    bool nextWave = false;
                    foreach (EnemyController enemy in EnemyManager.Instance.EnemyControllers)
                    {
                        if (enemy.isActiveAndEnabled)
                        {
                            nextWave = false;
                            break;
                        }
                        else
                        {
                            nextWave = true;
                        }
                    }
                    if (nextWave)
                    {
                        m_GameState.DecreaseWaveNumber();
                        if (m_MapData.EnemyWaves.Count > 1)
                        {
                            m_GameState.NormalEnemies = m_MapData.EnemyWaves.Dequeue();
                            m_GameState.StrongEnemies = m_MapData.EnemyWaves.Dequeue();
                        }
                        m_UseStrongEnemy = false;
                    }
                }
                else if (m_GameState.NormalEnemies == 0 && m_GameState.StrongEnemies == 0 && m_GameState.WaveNumber < 0)
                {
                    m_GameState.WinGame();
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

    private void Setup()
    {
        m_MapReaderMono.Displacement = m_TileDisplacement;
        m_MapReaderMono.Origin = m_Origin;
        m_MoveCamera.MoveCameraToOrigin(m_Origin);
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

    private void PlayMap()
    {
        EnemyManager.Instance.CreateEnemy(m_MapData.EnemySpawnWorldPosition, m_MapData.Path, m_CurrentEnemy);
    }

    private void ClearMap()
    {
        object[] oldMap = FindObjectsOfType<GameObject>();
        foreach(GameObject gameObject in oldMap)
        {
            if (gameObject.transform.parent == null && !gameObject.CompareTag(k_DontDestroy))
            {
                gameObject.SetActive(false);
            }
            //if(gameObject.CompareTag(k_ScriptablePool))
            //{
            //    gameObject.SetActive(true);
            //}
        }
        m_MapData.ClearLists();
        m_MoveCamera.ResetCameraPosition();
        EnemyManager.Instance.EnemyControllers.Clear();
    }
}
