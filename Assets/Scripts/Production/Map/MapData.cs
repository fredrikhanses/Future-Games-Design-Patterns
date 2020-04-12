using System.Collections.Generic;
using UnityEngine;

public interface IMapData
{
    void ClearLists();
}

public class MapData : IMapData
{
    public Vector2Int EnemySpawnTilePosition { get => m_EnemySpawnTilePosition; set => m_EnemySpawnTilePosition = value; }
    public Vector2Int PlayerBaseTilePosition { get => m_PlayerBaseTilePosition; set => m_PlayerBaseTilePosition = value; }
    public Vector3 EnemySpawnWorldPosition { get => m_EnemySpawnWorldPosition; set => m_EnemySpawnWorldPosition = value; }
    public Vector3 PlayerBaseWorldPosition { get => m_PlayerBaseWorldPosition; set => m_PlayerBaseWorldPosition = value; }
    public List<Vector2Int> WalkableTiles { get => m_WalkableTiles; set => m_WalkableTiles = value; }
    public LinkedList<Vector3> Path { get => m_Path; set => m_Path = value; }
    public List<KeyValuePair<Vector2Int, Vector3>> MapPositions { get => m_MapPositions; set => m_MapPositions = value; }
    public List<KeyValuePair<Vector3, GameObject>> MapLayout { get => m_MapLayout; set => m_MapLayout = value; }
    public Queue<int> EnemyWaves { get => m_EnemyWaves; set => m_EnemyWaves = value; }

    private Vector2Int m_EnemySpawnTilePosition = new Vector2Int();
    private Vector2Int m_PlayerBaseTilePosition = new Vector2Int();
    private Vector3 m_EnemySpawnWorldPosition = new Vector3();
    private Vector3 m_PlayerBaseWorldPosition = new Vector3();
    private List<Vector2Int> m_WalkableTiles = new List<Vector2Int>();
    private LinkedList<Vector3> m_Path = new LinkedList<Vector3>();
    private List<KeyValuePair<Vector2Int, Vector3>> m_MapPositions = new List<KeyValuePair<Vector2Int, Vector3>>();
    private List<KeyValuePair<Vector3, GameObject>> m_MapLayout = new List<KeyValuePair<Vector3, GameObject>>();
    private Queue<int> m_EnemyWaves = new Queue<int>();

    public void ClearLists()
    {
        m_WalkableTiles.Clear();
        m_Path.Clear();
        m_MapPositions.Clear();
        m_MapLayout.Clear();
    }
}