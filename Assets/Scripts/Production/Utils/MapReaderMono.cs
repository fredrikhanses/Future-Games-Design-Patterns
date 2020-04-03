﻿using System;
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
        [SerializeField, Tooltip("Cannot change during Play Mode")] private Vector3 origin = Vector3.zero;
        [SerializeField] private MapKeyDataMono[] mapReaderMonos;
        private List<MapKeyData> mapKeyData = new List<MapKeyData>();
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

        public Vector3 GetEnemySpawnWorldPosition()
        {
            return mapReader.GetEnemySpawnWorldPosition();
        }

        public Vector3 GetPlayerBaseWorldPosition()
        {
            return mapReader.GetPlayerBaseWorldPosition();
        }

        public List<Vector2Int> GetWalkableTiles()
        {
            return mapReader.GetWalkableTiles();
        }

        public Vector2Int GetPlayerBaseTilePosition()
        {
            return mapReader.GetPlayerBaseTilePosition();
        }

        public Vector2Int GetEnemySpawnTilePosition()
        {
            return mapReader.GetEnemySpawnTilePosition();
        }

        public List<KeyValuePair<Vector2Int, Vector3>> GetMapPositions()
        {
            return mapReader.GetMapPositions();
        }
    }
}