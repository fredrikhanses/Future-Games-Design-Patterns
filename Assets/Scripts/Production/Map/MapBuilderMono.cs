using AI;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class MapBuilderMono : MonoBehaviour
    {
        [SerializeField] private Vector3 origin = Vector3.zero;
        [SerializeField, Range(1, 4), Tooltip("Choose which map to generate")] private uint mapNumber = 3;
        [SerializeField, Tooltip("Only works in Play Mode")] private bool generateMap;
        [SerializeField, Tooltip("Only works in Play Mode")] private bool playMap;
        [SerializeField, Tooltip("Only works in Play Mode")] private bool clearMap;

        private Camera mainCamera;
        private MoveCamera moveCamera;
        private float tileDisplacement = 2.0f;
        private string mapName = "map_3";
        private MapReaderMono mapReaderMono;
        private IPathFinder pathFinder;
        private IEnumerable<Vector2Int> path;
        private MapData mapData = new MapData();
        private Dictionary<uint, string> Maps = new Dictionary<uint, string>
        {
            { 1, "map_1" },
            { 2, "map_2" },
            { 3, "map_3" },
            { 4, "map_4" }
        };

        private void Awake()
        {
            mapReaderMono = GetComponent<MapReaderMono>();
            mainCamera = Camera.main;
            moveCamera = mainCamera.GetComponent<MoveCamera>();
        }

        private void OnValidate()
        {
            if (generateMap && Application.isPlaying && mapData.MapLayout.Count <= 0)
            {
                GenerateMap();
                generateMap = false;
            }
            else
            {
                generateMap = false;
            }
            if (clearMap && Application.isPlaying && mapData.MapLayout.Count > 0)
            {
                ClearMap();
                clearMap = false;
            }
            else
            {
                clearMap = false;
            }
            if (playMap && Application.isPlaying && mapData.MapLayout.Count > 0)
            {
                PlayMap();
                playMap = false;
            }
            else
            {
                playMap = false;
            }
        }

        private void GenerateMap()
        {
            Setup();
            ReadMap();
            GeneratePath();
            CalculateWalkPoints(); 
            if (mapData.MapLayout != null && mapData.MapLayout.Count > 0)
            {
                foreach (KeyValuePair<Vector3, GameObject> objectPosition in mapData.MapLayout)
                {
                    Instantiate(objectPosition.Value, objectPosition.Key, Quaternion.identity);
                }
            }
            else
            {
                Debug.Log($"mapLayout: {mapData.MapLayout.Count}");
            }
        }

        private void Setup()
        {
            mapReaderMono.Displacement = tileDisplacement;
            mapReaderMono.Origin = origin;
            moveCamera.MoveCameraToOrigin(origin);
        }

        private void ReadMap()
        {
            mapName = Maps[mapNumber];
            mapData = mapReaderMono.ReadMap(mapName);
        }

        private void GeneratePath()
        {
            pathFinder = new Dijkstra(mapData.WalkableTiles);
            path = pathFinder.FindPath(mapData.EnemySpawnTilePosition, mapData.PlayerBaseTilePosition);
        }

        private void CalculateWalkPoints()
        {
            foreach (Vector2Int tilePosition in path)
            {
                foreach (KeyValuePair<Vector2Int, Vector3> mapPosition in mapData.MapPositions)
                {
                    if (tilePosition.Equals(mapPosition.Key))
                    {
                        mapData.WalkPoints.AddLast(mapPosition.Value);
                    }
                }
            }
        }

        private void PlayMap()
        {
            EnemyManager.Instance.CreateEnemy(mapData.EnemySpawnWorldPosition);
            EnemyManager.Instance.MoveStart(mapData.WalkPoints);
        }

        private void ClearMap()
        {
            object[] oldMap = FindObjectsOfType<GameObject>();
            foreach(GameObject gameObject in oldMap)
            {
                if (gameObject.transform.parent == null && !gameObject.CompareTag("DontDestroy"))
                {
                    Destroy(gameObject);
                }
            }
            mapData.ClearLists();
            moveCamera.ResetCameraPosition();
        }
    }
}