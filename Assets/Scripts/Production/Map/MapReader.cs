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

public interface IMapReader
{
    MapData ReadMap(string mapName);
}

public class MapReader : IMapReader
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
    private readonly char[] m_EnemyTypeSeparatorChar = { ' ' };
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
        string map = GetMap(mapName);
        string[] lines = map.Split(m_LineSeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            line.ToCharArray();
        }
        int mapSizeX = lines[0].Length;
        int mapSizeY = lines.Length;
        for (int y = mapSizeY - 1; y >= 0; y--)
        {
            for (int x = 0; x < mapSizeX - 1; x++)
            {
                m_CurrentWorldPosition.x = x * m_Displacement + m_Origin.x;
                m_CurrentWorldPosition.y = m_Origin.y;
                m_CurrentWorldPosition.z = -y * m_Displacement + m_Origin.z;
                int id = (int)char.GetNumericValue(lines[y][x]);
                if (id.Equals((int)WalkableTileNames.Path) || id.Equals((int)WalkableTileNames.Start) || id.Equals((int)WalkableTileNames.End))
                {
                    m_CurrentWalkableTilePosition.x = x;
                    m_CurrentWalkableTilePosition.y = -y;
                    m_MapData.WalkableTiles.Add(m_CurrentWalkableTilePosition);
                    m_MapData.MapPositions.Add(new KeyValuePair<Vector2Int, Vector3>(m_CurrentWalkableTilePosition, m_CurrentWorldPosition));
                    if (id.Equals((int)WalkableTileNames.Start))
                    {
                        m_EnemySpawnWorldPosition = m_CurrentWorldPosition;
                        m_EnemySpawnWorldPosition.y += m_GroundHeight;
                        m_MapData.EnemySpawnWorldPosition = m_EnemySpawnWorldPosition;
                        m_MapData.EnemySpawnTilePosition = m_CurrentWalkableTilePosition;
                    }
                    if (id.Equals((int)WalkableTileNames.End))
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
                    TileType tileType = TileMethods.TypeById[id];
                    m_MapData.MapLayout.Add(new KeyValuePair<Vector3, GameObject>(m_CurrentWorldPosition, m_PrefabsById[tileType]));
                }
            }
        } 
        return m_MapData;
    }

    private string GetMap(string mapName)
    {
        string mapContent = m_TextHandler.ReadText(mapName);
        // Copy map
        // m_TextHandler.WriteText("map_5", mapContent);
        string[] mapHolder = mapContent.Split(m_MapSeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        CalculateWaveData(mapHolder[1]);
        return mapHolder[0];
    }

    private void CalculateWaveData(string waveData)
    {
        string[] waves = waveData.Split(m_LineSeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        foreach (string wave in waves)
        {
            string[] numberInWave = wave.Split(m_EnemyTypeSeparatorChar, StringSplitOptions.RemoveEmptyEntries);
            foreach (string number in numberInWave)
            {
                if(int.TryParse(number, out int result))
                {
                    m_MapData.EnemyWaves.Enqueue(result);
                }
            }
        }
    }
}