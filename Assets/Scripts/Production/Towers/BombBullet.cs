using Tools;
using UnityEngine;

class BombBullet : MonoBehaviour, IBullet
{
    [SerializeField] private GameObjectScriptablePool m_ExplosionScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_ExplosionRadiusScriptablePool;

    public void OnTriggerEnter(Collider other)
    {
        if (m_ExplosionScriptablePool != null && m_ExplosionRadiusScriptablePool != null)
        {
            m_ExplosionScriptablePool.Rent(true).transform.position = transform.position;
            m_ExplosionRadiusScriptablePool.Rent(true).transform.position = transform.position;
        }
    }
}
