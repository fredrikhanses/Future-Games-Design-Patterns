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
    private readonly Dictionary<TileType, GameObject> m_PrefabsById;
    private readonly float m_Displacement = 1f;
    private readonly Vector3 m_Origin;

    /// <summary>
    ///    Only used for navigation tests. If building map, use overloaded method.
    /// </summary>
    public MapReader() { }

    public MapReader(Vector3 origin, float displacement, IEnumerable<MapKeyData> mapKeyData)
    {
        m_Displacement = displacement;
        m_Origin = origin;
        m_PrefabsById = new Dictionary<TileType, GameObject>();
        foreach (MapKeyData data in mapKeyData)
        {
            m_PrefabsById.Add(data.TileType, data.Prefab);
        }
    }

    private readonly char[] m_MapSeparatorChar = { '#' };
    private readonly char[] m_LineSeparatorChar = { '\n' };
    private string m_MapContent;
    private string[] m_MapHolder;
    private int m_MapSizeX;
    private int m_MapSizeY;
    private int m_Id;
    private float m_GroundHeight = 0.8f;
    private readonly TextHandler m_TextHandler = new TextHandler();
    private readonly MapData m_MapData = new MapData();
    private Vector3 m_EnemySpawnWorldPosition = new Vector3();
    private Vector3 m_CurrentWorldPosition = new Vector3();
    private Vector2Int m_CurrentWalkableTilePosition = new Vector2Int();
 
    private enum WalkableTileNames
    {
        Path,
        Start = 8,
        End
    }

    /// <summary>Reads and interprets a map.</summary>
    /// <param name="mapName">Name of the map to read.</param>
    /// <returns>MapData containing all necessary data for building a map.</returns>
    public MapData ReadMap(string mapName)
    {
        m_MapData.ClearLists();
        m_MapContent = GetMap(mapName);
        m_MapHolder = m_MapContent.Split(m_LineSeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in m_MapHolder)
        {
            line.ToCharArray();
        }
        m_MapSizeX = m_MapHolder[0].Length;
        m_MapSizeY = m_MapHolder.Length;
        for (int y = m_MapSizeY - 1; y >= 0; y--)
        {
            for (int x = 0; x < m_MapSizeX - 1; x++)
            {
                m_CurrentWorldPosition.x = x * m_Displacement + m_Origin.x;
                m_CurrentWorldPosition.y = m_Origin.y;
                m_CurrentWorldPosition.z = -y * m_Displacement + m_Origin.z;
                m_Id = (int)char.GetNumericValue(m_MapHolder[y][x]);
                if (m_Id.Equals((int)WalkableTileNames.Path) || m_Id.Equals((int)WalkableTileNames.Start) || m_Id.Equals((int)WalkableTileNames.End))
                {
                    m_CurrentWalkableTilePosition.x = x;
                    m_CurrentWalkableTilePosition.y = -y;
                    m_MapData.WalkableTiles.Add(m_CurrentWalkableTilePosition);
                    m_MapData.MapPositions.Add(new KeyValuePair<Vector2Int, Vector3>(m_CurrentWalkableTilePosition, m_CurrentWorldPosition));
                    if (m_Id.Equals((int)WalkableTileNames.Start))
                    {
                        m_EnemySpawnWorldPosition = m_CurrentWorldPosition;
                        m_EnemySpawnWorldPosition.y += m_GroundHeight;
                        m_MapData.EnemySpawnWorldPosition = m_EnemySpawnWorldPosition;
                        m_MapData.EnemySpawnTilePosition = m_CurrentWalkableTilePosition;
                    }
                    if (m_Id.Equals((int)WalkableTileNames.End))
                    {
                        m_MapData.PlayerBaseWorldPosition = m_CurrentWorldPosition;
                        m_MapData.PlayerBaseTilePosition = m_CurrentWalkableTilePosition;
                    }
                }
                /// <summary>
                ///    Check needed because of unit tests.
                /// </summary>
                if (m_PrefabsById != null)
                {
                    TileType tileType = TileMethods.TypeById[m_Id];
                    m_MapData.MapLayout.Add(new KeyValuePair<Vector3, GameObject>(m_CurrentWorldPosition, m_PrefabsById[tileType]));
                }
            }
        } 
        return m_MapData;
    }

    private string GetMap(string mapName)
    {
        m_MapContent = m_TextHandler.ReadText(mapName);
        m_MapHolder = m_MapContent.Split(m_MapSeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        return m_MapHolder[0];
    }
}