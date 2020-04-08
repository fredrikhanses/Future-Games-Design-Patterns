//using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private EnemyController m_CurrentEnemy;
    //private EnemyController m_PotentialEnemy;
    [SerializeField] private Weapon m_Weapon;
    [SerializeField] private GameObject m_TowerTop;
    [SerializeField] private float m_MaxRange = 10f;
    //private List<EnemyController> m_EnemyList = new List<EnemyController>();
    //private bool first = true;
    private float m_ShortestDistanceSqr = float.MaxValue;
    private Vector3 m_LookDirection;
    private float m_ShootTimer = 0f;
    //private float m_AimTimer = 0f;

    void FixedUpdate()
    {
        //m_AimTimer -= Time.fixedDeltaTime;
        if(/*m_AimTimer <= 0f &&*/ m_CurrentEnemy == null || m_CurrentEnemy.isActiveAndEnabled == false || (m_CurrentEnemy.transform.position - m_TowerTop.transform.position).sqrMagnitude > (m_MaxRange * m_MaxRange))
        {
            foreach (EnemyController enemy in FindObjectsOfType<EnemyController>())
            {
                float distanceSqr = (enemy.transform.position - m_TowerTop.transform.position).sqrMagnitude;
                if (distanceSqr < m_ShortestDistanceSqr)
                {
                    m_ShortestDistanceSqr = distanceSqr;
                    m_CurrentEnemy = enemy;
                }
            }
            m_ShortestDistanceSqr = float.MaxValue;
            //m_AimTimer = Random.Range(1f, 2f);
        }

        //m_PotentialEnemy =  FindObjectOfType<EnemyController>();
        
        //if(first)
        //{
        //    m_CurrentEnemy = m_PotentialEnemy;
        //    if(m_CurrentEnemy)
        //    {
        //        first = false;
        //    }
        //}
        //if(m_PotentialEnemy != null)
        //{
        //    if ((m_PotentialEnemy.transform.position - m_TowerTop.transform.position).sqrMagnitude < (m_CurrentEnemy.transform.position - m_TowerTop.transform.position).sqrMagnitude)
        //    {
        //        m_CurrentEnemy = m_PotentialEnemy;
        //    }
        //}

        if(m_CurrentEnemy != null && m_CurrentEnemy.isActiveAndEnabled && (m_CurrentEnemy.transform.position - m_TowerTop.transform.position).sqrMagnitude <= (m_MaxRange * m_MaxRange))
        {
            m_LookDirection = (m_CurrentEnemy.transform.position - m_TowerTop.transform.position).normalized;
            m_TowerTop.transform.rotation = Quaternion.LookRotation(m_LookDirection);

            m_ShootTimer -= Time.fixedDeltaTime;
            if (m_ShootTimer <= 0f)
            {
                m_Weapon.Shoot(m_LookDirection);
                m_ShootTimer = Random.Range(1f, 3f);
            }
        }
        //else
        //{
        //    first = true;
        //}
    }
}
