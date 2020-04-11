using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] private Weapon m_Weapon;
    [SerializeField] private GameObject m_TowerTop;
    [SerializeField] private float m_MaxRange = 20f;
    
    private EnemyController m_CurrentEnemy;
    private float m_ShootTimer = 0f;

    private void UpdateClosestTarget()
    {
        float shortestDistanceSqr = float.MaxValue;
        foreach (EnemyController enemy in EnemyManager.Instance.EnemyControllers)
        {
            if (enemy.isActiveAndEnabled)
            {
                float distanceSqr = (enemy.transform.position - m_TowerTop.transform.position).sqrMagnitude;
                if (distanceSqr <= m_MaxRange * m_MaxRange && distanceSqr < shortestDistanceSqr)
                {
                    shortestDistanceSqr = distanceSqr;
                    m_CurrentEnemy = enemy;
                }
            }
        }
    }

    void FixedUpdate()
    {
        // Set first closest target.
        if(m_CurrentEnemy == null && EnemyManager.Instance.EnemyControllers.Count > 0)
        {
            UpdateClosestTarget();
        }
        // Set new closest target when current target is dead.
        if (m_CurrentEnemy != null && m_CurrentEnemy.isActiveAndEnabled == false && EnemyManager.Instance.EnemyControllers.Count > 0)
        {
            UpdateClosestTarget();
        }

        // Current target is active.
        if (m_CurrentEnemy != null && m_CurrentEnemy.isActiveAndEnabled)
        {
            Vector3 m_LookDirection = m_CurrentEnemy.transform.position - m_TowerTop.transform.position;
            // Set new closest target when current target is out of range.
            if (m_LookDirection.sqrMagnitude > (m_MaxRange * m_MaxRange) && EnemyManager.Instance.EnemyControllers.Count > 0)
            {
                UpdateClosestTarget();
            }
            // Face current target when target is in range. Then shoot at it if able.
            if (m_LookDirection.sqrMagnitude <= (m_MaxRange * m_MaxRange))
            {
                m_TowerTop.transform.rotation = Quaternion.LookRotation(m_LookDirection.normalized);
                m_ShootTimer -= Time.fixedDeltaTime;
                if (m_ShootTimer <= 0f)
                {
                    m_Weapon.Shoot(m_LookDirection);
                    m_ShootTimer = Random.Range(1f, 3f);
                }
            }
        }
    }
}
