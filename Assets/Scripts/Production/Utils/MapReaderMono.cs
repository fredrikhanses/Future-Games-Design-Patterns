using System;
using System.Collections.Generic;
using UnityEngine;
using Tools;

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
    private TextHandler textHandler = new TextHandler();

    [SerializeField]
    private MapKeyDataMono[] mapReaderMonos;

    private List<MapKeyData> mapKeyData = new List<MapKeyData>();

    [SerializeField, Range(1, 3), Tooltip("Choose which map to generate")]
    private uint mapNumber = 3;

    private Dictionary<uint, string> Maps = new Dictionary<uint, string> 
    {
        { 1,  "map_1" },
        { 2,  "map_2" },
        { 3,  "map_3" },
    };

    private string map = "map_3";

    [SerializeField, Tooltip("Only works in Play Mode")]
    private bool generateMap;

    //[SerializeField, Tooltip("Only works in Play Mode")]
    //private bool clearMap;

    private char mapSeparatorChar = '#';
    private char lineSeparatorChar = '\n';

    private uint mapSizeX;
    private uint mapSizeY;

    //List<GameObject> objectPool = new List<GameObject>();

    private void Awake()
    {
        foreach (MapKeyDataMono readerMono in mapReaderMonos)
        {
            MapKeyData data = new MapKeyData(readerMono.TileType, readerMono.Prefab);
            mapKeyData.Add(data);
        }
    }

    private void OnValidate()
    {
        if(generateMap && Application.isPlaying)
        {
            map = Maps[mapNumber];
            GetMap(map);
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

    private void GetMap(string mapName)
    {
        string mapContent = textHandler.ReadText(mapName);
        Debug.Log(mapContent);
        string[] mapContentArray = mapContent.Split(mapSeparatorChar);
        Debug.Log(mapContentArray[0]);
        GenerateMap(mapContentArray[0]);
    }

    private void GenerateMap(string map)
    {
        string[] lineArray = map.Split(lineSeparatorChar);
        Debug.Log(lineArray[0]);
        foreach (string line in lineArray)
        {
            line.ToCharArray();
        }
        mapSizeX = (uint)lineArray[0].Length - 1;
        mapSizeY = (uint)lineArray.Length - 1;
        for (int j = 0; j < mapSizeY; j++)
        {
            for (int i = 0; i < mapSizeX; i++)
            {
                char currentChar = lineArray[j][i];
                Debug.Log(currentChar);
                TileType tileType = TileMethods.TypeByIdChar[currentChar];
                MapReader mapReader = new MapReader(mapKeyData: mapKeyData);
                GameObject currentPrefab = mapReader.m_PrefabsById[tileType];
                Instantiate(currentPrefab, new Vector3(-i * 2f, 0, j * 2f), Quaternion.identity);
                //objectPool.Add(currentPrefab);
            }
        }
    }
}