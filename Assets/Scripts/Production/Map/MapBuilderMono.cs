using AI;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class MapBuilderMono : MonoBehaviour
    {
        [SerializeField] private Vector3 m_Origin = Vector3.zero;
        [SerializeField, Range(1, 4), Tooltip("Choose which map to generate")] private uint m_MapNumber = 4;
        [SerializeField, Tooltip("Only works in Play Mode")] private bool m_GenerateMap;
        [SerializeField, Tooltip("Only works in Play Mode")] private bool m_PlayMap;
        [SerializeField, Tooltip("Only works in Play Mode")] private bool m_ClearMap;
        [SerializeField] private GameObjectScriptablePool m_PathTileScriptablePool;
        [SerializeField] private GameObjectScriptablePool m_ObstacleTileScriptablePool;
        [SerializeField] private GameObjectScriptablePool m_BombTowerScriptablePool;
        [SerializeField] private GameObjectScriptablePool m_FreezeTowerScriptablePool;
        [SerializeField] private GameObjectScriptablePool m_EnemyBaseScriptablePool;
        [SerializeField] private GameObjectScriptablePool m_PlayerBaseScriptablePool;

        private float m_SpawnInterval = 1f;
        private float m_TileDisplacement = 2.0f;
        private string m_MapName;
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
            { 4, "map_4" }
        };
        private const string k_DontDestroy = "DontDestroy";

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
        }

        private void Start()
        {
            m_MapReaderMono = GetComponent<MapReaderMono>();
            m_MainCamera = Camera.main;
            m_MoveCamera = m_MainCamera.GetComponent<MoveCamera>();
            GenerateMap();
        }

        private void FixedUpdate()
        {
            if (m_MapData.MapLayout.Count > 0)
            {
                m_SpawnInterval -= Time.fixedDeltaTime;
                if (m_SpawnInterval <= 0f)
                {
                    PlayMap();
                    m_SpawnInterval = Random.Range(1f, 3f);
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
            EnemyManager.Instance.CreateEnemy(m_MapData.EnemySpawnWorldPosition, m_MapData.Path);
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
            }
            m_MapData.ClearLists();
            m_MoveCamera.ResetCameraPosition();
        }
    }
}