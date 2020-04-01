using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    [Serializable]
    public struct MapKeyDataMono
    {
        [SerializeField]
        private TileType tileType;

        [SerializeField]
        private GameObject prefab;

        public TileType TileType => tileType;
        public GameObject Prefab => prefab;
    }

    public class MapReaderMono : MonoBehaviour
    {
        [SerializeField]
        private MapKeyDataMono[] mapReaderMonos;
        private List<MapKeyData> mapKeyData = new List<MapKeyData>();
        private Vector3 origin = Vector3.zero;
        private float displacement = 2.0f;
        private MapReader mapReader;

        private void Awake()
        {
            foreach (MapKeyDataMono readerMono in mapReaderMonos)
            {
                MapKeyData data = new MapKeyData(readerMono.TileType, readerMono.Prefab);
                mapKeyData.Add(data);
            }
            mapReader = new MapReader(origin, displacement, mapKeyData: mapKeyData);
        }

        public List<KeyValuePair<Vector3, GameObject>> ReadMap(string mapName)
        {
            return mapReader.ReadMap(mapName);
        }
    }
}