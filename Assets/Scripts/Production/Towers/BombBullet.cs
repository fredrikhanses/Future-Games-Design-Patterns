using Tools;
using UnityEngine;

class BombBullet : MonoBehaviour, ITriggerable
{
    [SerializeField] private GameObjectScriptablePool m_ExplosionScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_ExplosionRadiusScriptablePool;

    /// <summary>
    ///     Spawn explosion particles and explosion radius at impact position.
    /// </summary>
    public void OnTriggerEnter()
    {
        if (m_ExplosionScriptablePool != null && m_ExplosionRadiusScriptablePool != null)
        {
            m_ExplosionScriptablePool.Rent(true).transform.position = transform.position;
            m_ExplosionRadiusScriptablePool.Rent(true).transform.position = transform.position;
        }
    }
}
