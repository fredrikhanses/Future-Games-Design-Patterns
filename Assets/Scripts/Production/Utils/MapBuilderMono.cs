using AI;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class MapBuilderMono : MonoBehaviour
    {
        [SerializeField, Range(1, 3), Tooltip("Choose which map to generate")]
        private uint mapNumber = 3;
        private Dictionary<uint, string> Maps = new Dictionary<uint, string>
        {
            { 1,  "map_1" },
            { 2,  "map_2" },
            { 3,  "map_3" },
        };
        private string mapName = "map_3";
        [SerializeField, Tooltip("Only works in Play Mode")]
        private bool generateMap;
        [SerializeField, Tooltip("Only works in Play Mode")]
        private bool playMap;
        [SerializeField, Tooltip("Only works in Play Mode")]
        private bool clearMap;
        //List<GameObject> objectPool = new List<GameObject>();
        private MapReaderMono mapReaderMono;
        private List<KeyValuePair<Vector3, GameObject>> mapLayout = new List<KeyValuePair<Vector3, GameObject>>();
        IPathFinder pathFinder;
        IEnumerable<Vector2Int> path;

        private void Awake()
        {
            mapReaderMono = GetComponent<MapReaderMono>();
        }

        private void OnValidate()
        {
            if (generateMap && Application.isPlaying && mapLayout.Count <= 0)
            {
                mapName = Maps[mapNumber];
                mapLayout = mapReaderMono.ReadMap(mapName);
                GenerateMap();
                GeneratePath();
                generateMap = false;
            }
            else
            {
                generateMap = false;
            }
            if (clearMap && Application.isPlaying && mapLayout.Count > 0)
            {
                ClearMap();
                clearMap = false;
            }
            else
            {
                clearMap = false;
            }
            if (playMap && Application.isPlaying && mapLayout.Count > 0)
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
            if (mapLayout != null && mapLayout.Count > 0)
            {
                foreach (KeyValuePair<Vector3, GameObject> objectPosition in mapLayout)
                {
                    Instantiate(objectPosition.Value, objectPosition.Key, Quaternion.identity);
                    //objectPool.Add(currentPrefab);
                }
            }
            else
            {
                Debug.Log($"mapLayout: {mapLayout.Count}");
            }
        }

        private void GeneratePath()
        {
            pathFinder = new Dijkstra(mapReaderMono.GetWalkableTiles());
            path = pathFinder.FindPath(mapReaderMono.GetEnemySpawnTilePosition(), mapReaderMono.GetPlayerBaseTilePosition());
        }

        private void PlayMap()
        {
            EnemyManager.Instance.CreateEnemy(mapReaderMono.GetEnemySpawnWorldPosition());
            EnemyManager.Instance.Move(path, mapReaderMono.GetMapPositions());
            //EnemyManager.Instance.MoveEnemy(mapReaderMono.GetPlayerBaseWorldPosition());
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
            mapLayout.Clear();
        }
    }
}