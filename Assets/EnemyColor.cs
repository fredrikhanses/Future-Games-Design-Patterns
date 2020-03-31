using UnityEngine;

public class EnemyColor : MonoBehaviour
{
    [ContextMenu("Call EnemyManager")]
    public void CallEnemyManager()
    {
        EnemyManager.Instance.Log();
    }
}
