using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    private static EnemyManager m_Instance;

    public static EnemyManager Instance
    {
        get
        {
            if(m_Instance == null)
            {
                EnemyManager[] managers = GameObject.FindObjectsOfType<EnemyManager>();
                if(managers.Length > 1)
                {
                    throw new InvalidOperationException("Only one instance of EnemyManager must be placed in the scene");
                }

                if(managers.Length > 0)
                {
                    m_Instance = managers[0];
                }
                else
                {
                    GameObject prefab = Resources.Load<GameObject>("EnemyManager");
                    GameObject obj = Instantiate(prefab);
                    m_Instance = obj.GetComponent<EnemyManager>();
                }
            }
            return m_Instance;
        }
    }

    public void Log()
    {
        Debug.Log(enemyPrefab.name);
    }
}
