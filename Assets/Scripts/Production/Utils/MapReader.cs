using System.Collections.Generic;
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
    public readonly Dictionary<TileType, GameObject> m_PrefabsById;
    public MapReader(IEnumerable<MapKeyData> mapKeyData)
    {
        m_PrefabsById = new Dictionary<TileType, GameObject>();
        foreach (MapKeyData data in mapKeyData)
        {
            m_PrefabsById.Add(data.TileType, data.Prefab);
        }
    }
}
