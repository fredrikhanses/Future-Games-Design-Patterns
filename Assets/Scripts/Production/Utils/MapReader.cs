using System;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;

public struct MapKeyData
{
    public TileType TileType { get; private set; }
    public GameObject Prefab { get; private set; }
    public MapKeyData(TileType tileType, GameObject prefab)
    {
        TileType = tileType;
        Prefab = prefab;
    }
}
public class MapReader
{
    private readonly Dictionary<TileType, GameObject> m_PrefabsById; // Change to private
    private readonly float displacement;
    private readonly Vector3 origin;

    /// <summary>
    ///    Only used for navigation tests. If building map, use overloaded method.
    /// </summary>
    public MapReader() { }

    public MapReader(Vector3 origin, float displacement, IEnumerable<MapKeyData> mapKeyData)
    {
        this.displacement = displacement;
        this.origin = origin;
        m_PrefabsById = new Dictionary<TileType, GameObject>();
        foreach (MapKeyData data in mapKeyData)
        {
            m_PrefabsById.Add(data.TileType, data.Prefab);
        }
    }

    private char[] mapSeparatorChar = { '#' };
    private char[] lineSeparatorChar = { '\n' };
    private string mapContent;
    private string[] mapHolder;
    private int mapSizeX;
    private int mapSizeY;
    private int id;
    private List<Vector2Int> walkableTiles = new List<Vector2Int>();
    private List<KeyValuePair<Vector3, GameObject>> mapLayout = new List<KeyValuePair<Vector3, GameObject>>();
    private List<KeyValuePair<Vector3, GameObject>> returnMapLayout = new List<KeyValuePair<Vector3, GameObject>>();
    private TextHandler textHandler = new TextHandler();
    private enum WalkableTileNames
    {
        Path,
        Start = 8,
        End
    }

    public List<KeyValuePair<Vector3, GameObject>> ReadMap(string mapName)
    {
        mapContent = GetMap(mapName);
        mapHolder = mapContent.Split(lineSeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in mapHolder)
        {
            Debug.Log($"line: {line}");
            line.ToCharArray();
        }
        mapSizeX = mapHolder[0].Length;
        mapSizeY = mapHolder.Length;
        for (int y = mapSizeY - 1; y >= 0; y--)
        {
            for (int x = 0; x < mapSizeX - 1; x++)
            {
                id = (int)char.GetNumericValue(mapHolder[y][x]);
                Debug.Log($"x: {x}, y: {y}, id: {id}");
                if (id.Equals((int)WalkableTileNames.Path) || id.Equals((int)WalkableTileNames.Start) || id.Equals((int)WalkableTileNames.End))
                {
                    walkableTiles.Add(new Vector2Int(x, y));
                }
                if(m_PrefabsById != null)
                {
                    TileType tileType = TileMethods.TypeById[id];
                    mapLayout.Add(new KeyValuePair<Vector3, GameObject>(new Vector3(x * displacement, 0, -y * displacement), m_PrefabsById[tileType]));
                }
            }
        }
        foreach(KeyValuePair<Vector3, GameObject> objectPosition in mapLayout)
        {
            returnMapLayout.Add(objectPosition);
        }
        mapLayout.Clear();
        return returnMapLayout;
    }

    private string GetMap(string mapName)
    {
        mapContent = textHandler.ReadText(mapName);
        Debug.Log(mapContent);
        mapHolder = mapContent.Split(mapSeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        Debug.Log($"{mapHolder[0]}{mapHolder[1]}");
        return mapHolder[0];
    }
    public List<Vector2Int> GetWalkableTiles()
    {
        return walkableTiles;
    }
}