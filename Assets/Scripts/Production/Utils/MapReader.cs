using System;
using System.Collections.Generic;
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

    private readonly char[] mapSeparatorChar = { '#' };
    private readonly char[] lineSeparatorChar = { '\n' };
    private string mapContent;
    private string[] mapHolder;
    private int mapSizeX;
    private int mapSizeY;
    private int id;
    private float groundHeight = 1f;
    private readonly TextHandler textHandler = new TextHandler();
    private readonly MapData mapData = new MapData();
    private Vector3 enemySpawnWorldPosition = new Vector3();
    private Vector3 currentWorldPosition = new Vector3();
    private Vector2Int currentWalkableTilePosition = new Vector2Int();
 
    private enum WalkableTileNames
    {
        Path,
        Start = 8,
        End
    }

    public MapData ReadMap(string mapName)
    {
        mapContent = GetMap(mapName);
        mapHolder = mapContent.Split(lineSeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in mapHolder)
        {
            //Debug.Log($"line: {line}");
            line.ToCharArray();
        }
        mapSizeX = mapHolder[0].Length;
        mapSizeY = mapHolder.Length;
        for (int y = mapSizeY - 1; y >= 0; y--)
        {
            for (int x = 0; x < mapSizeX - 1; x++)
            {
                currentWorldPosition.x = x * displacement + origin.x;
                currentWorldPosition.y = origin.y;
                currentWorldPosition.z = -y * displacement + origin.z;
                id = (int)char.GetNumericValue(mapHolder[y][x]);
                //Debug.Log($"x: {x}, y: {y}, id: {id}");
                if (id.Equals((int)WalkableTileNames.Path) || id.Equals((int)WalkableTileNames.Start) || id.Equals((int)WalkableTileNames.End))
                {
                    currentWalkableTilePosition.x = x;
                    currentWalkableTilePosition.y = -y;
                    mapData.WalkableTiles.Add(currentWalkableTilePosition);
                    mapData.MapPositions.Add(new KeyValuePair<Vector2Int, Vector3>(currentWalkableTilePosition, currentWorldPosition));
                    if (id.Equals((int)WalkableTileNames.Start))
                    {
                        enemySpawnWorldPosition = currentWorldPosition;
                        enemySpawnWorldPosition.y = groundHeight;
                        mapData.EnemySpawnWorldPosition = enemySpawnWorldPosition;
                        mapData.EnemySpawnTilePosition = currentWalkableTilePosition;
                    }
                    if (id.Equals((int)WalkableTileNames.End))
                    {
                        mapData.PlayerBaseWorldPosition = currentWorldPosition;
                        mapData.PlayerBaseTilePosition = currentWalkableTilePosition;
                    }
                }
                /// <summary>
                ///    Check needed because of unit tests.
                /// </summary>
                if (m_PrefabsById != null)
                {
                    TileType tileType = TileMethods.TypeById[id];
                    mapData.MapLayout.Add(new KeyValuePair<Vector3, GameObject>(currentWorldPosition, m_PrefabsById[tileType]));
                }
            }
        } 
        return mapData;
    }

    private string GetMap(string mapName)
    {
        mapContent = textHandler.ReadText(mapName);
        //Debug.Log(mapContent);
        mapHolder = mapContent.Split(mapSeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        //Debug.Log($"{mapHolder[0]}{mapHolder[1]}");
        return mapHolder[0];
    }
}