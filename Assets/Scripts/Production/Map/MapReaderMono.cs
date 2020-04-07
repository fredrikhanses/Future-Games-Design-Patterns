using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    [Serializable]
    public struct MapKeyDataMono
    {
        [SerializeField] private TileType tileType;
        [SerializeField] private GameObject prefab;

        public TileType TileType => tileType;
        public GameObject Prefab => prefab;
    }

    public class MapReaderMono : MonoBehaviour
    {
       
        [SerializeField] private MapKeyDataMono[] mapReaderMonos;

        public Vector3 Origin { get => origin; set => origin = value; }
        public float Displacement { get => displacement; set => displacement = value; }
        
        private float displacement = 2.0f;
        private Vector3 origin = Vector3.zero;
        private List<MapKeyData> m_MapKeyData = new List<MapKeyData>();
        private MapReader mapReader;

        private void Awake()
        {
            foreach (MapKeyDataMono readerMono in mapReaderMonos)
            {
                MapKeyData data = new MapKeyData(readerMono.TileType, readerMono.Prefab);
                m_MapKeyData.Add(data);
            }
        }

        public MapData ReadMap(string mapName)
        {
            mapReader = new MapReader(Origin, Displacement, m_MapKeyData);
            return mapReader.ReadMap(mapName);
        }
    }
}