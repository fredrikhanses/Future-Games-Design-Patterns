using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    [Serializable]
    public struct MapKeyDataMono
    {
        [SerializeField] private TileType m_TileType;
        [SerializeField] private GameObject m_Prefab;

        public TileType TileType => m_TileType;
        public GameObject Prefab => m_Prefab;
    }

    public class MapReaderMono : MonoBehaviour, IMapReader
    {
       
        [SerializeField] private MapKeyDataMono[] m_MapReaderMonos;

        public Vector3 Origin { get => m_Origin; set => m_Origin = value; }
        public float Displacement { get => m_Displacement; set => m_Displacement = value; }
        
        private float m_Displacement = 2.0f;
        private Vector3 m_Origin = Vector3.zero;
        private List<MapKeyData> m_MapKeyData = new List<MapKeyData>();
        private MapReader m_MapReader;

        private void Awake()
        {
            foreach (MapKeyDataMono readerMono in m_MapReaderMonos)
            {
                MapKeyData mapKeyData = new MapKeyData(readerMono.TileType, readerMono.Prefab);
                m_MapKeyData.Add(mapKeyData);
            }
        }

        public MapData ReadMap(string mapName)
        {
            m_MapReader = new MapReader(Origin, Displacement, m_MapKeyData);
            return m_MapReader.ReadMap(mapName);
        }
    }
}