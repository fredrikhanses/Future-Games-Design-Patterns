using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public Vector2Int EnemySpawnTilePosition { get => enemySpawnTilePosition; set => enemySpawnTilePosition = value; }
    public Vector2Int PlayerBaseTilePosition { get => playerBaseTilePosition; set => playerBaseTilePosition = value; }
    public Vector3 EnemySpawnWorldPosition { get => enemySpawnWorldPosition; set => enemySpawnWorldPosition = value; }
    public Vector3 PlayerBaseWorldPosition { get => playerBaseWorldPosition; set => playerBaseWorldPosition = value; }
    public List<Vector2Int> WalkableTiles { get => walkableTiles; set => walkableTiles = value; }
    public LinkedList<Vector3> WalkPoints { get => walkPoints; set => walkPoints = value; }
    public List<KeyValuePair<Vector2Int, Vector3>> MapPositions { get => mapPositions; set => mapPositions = value; }
    public List<KeyValuePair<Vector3, GameObject>> MapLayout { get => mapLayout; set => mapLayout = value; }
    
    private Vector2Int enemySpawnTilePosition = new Vector2Int();
    private Vector2Int playerBaseTilePosition = new Vector2Int();
    private Vector3 enemySpawnWorldPosition = new Vector3();
    private Vector3 playerBaseWorldPosition = new Vector3();
    private List<Vector2Int> walkableTiles = new List<Vector2Int>();
    private LinkedList<Vector3> walkPoints = new LinkedList<Vector3>();
    private List<KeyValuePair<Vector2Int, Vector3>> mapPositions = new List<KeyValuePair<Vector2Int, Vector3>>();
    private List<KeyValuePair<Vector3, GameObject>> mapLayout = new List<KeyValuePair<Vector3, GameObject>>();
    
    public void ClearLists()
    {
        walkableTiles.Clear();
        WalkPoints.Clear();
        mapPositions.Clear();
        mapLayout.Clear();
    }
}