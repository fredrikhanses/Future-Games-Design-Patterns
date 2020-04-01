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
        //[SerializeField, Tooltip("Only works in Play Mode")]
        //private bool clearMap;
        //List<GameObject> objectPool = new List<GameObject>();
        private MapReaderMono mapReaderMono;
        private List<KeyValuePair<Vector3, GameObject>> mapLayout = new List<KeyValuePair<Vector3, GameObject>>();

        private void Awake()
        {
            // TODO: Fetch mapKeyData from MapReaderMono
            mapReaderMono = GetComponent<MapReaderMono>();
        }

        private void OnValidate()
        {
            if (generateMap && Application.isPlaying)
            {
                mapName = Maps[mapNumber];
                mapLayout = mapReaderMono.ReadMap(mapName);
                GenerateMap();
                generateMap = false;
            }
            else
            {
                generateMap = false;
            }
            //if (clearMap && Application.isPlaying)
            //{
            //    clearMap = false;
            //}
            //else
            //{
            //    clearMap = false;
            //}
        }

        public void GenerateMap()
        {
            if(mapLayout != null && mapLayout.Count > 0)
            {
                foreach(KeyValuePair<Vector3, GameObject> objectPosition in mapLayout)
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
    }
}
