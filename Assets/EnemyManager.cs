using UnityEngine;

[SingletonConfiguration(resourcesPath:"EnemyManager")]
public class EnemyManager : MonoSingleton<EnemyManager>
{
    [SerializeField]
    private GameObject enemyPrefab;

    public void CreateEnemy()
    {
        Instantiate(enemyPrefab);
    }
}
