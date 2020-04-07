using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    private EnemyController m_CurrentEnemy;
    private EnemyController m_PotentialEnemy;
    [SerializeField] private Weapon m_Weapon;
    [SerializeField] private GameObject m_TowerTop;
    private List<EnemyController> m_EnemyList = new List<EnemyController>();
    private bool first = true;
    private float shortestDistanceSqr = float.MaxValue;
    private Vector3 m_LookDirection;
    private float m_ShootTimer = 0f;
    private float m_AimTimer = 0f;
    private float m_MaxRange = 100f;

    void FixedUpdate()
    {
        //m_AimTimer -= Time.fixedDeltaTime;
        if(/*m_AimTimer <= 0f &&*/ m_CurrentEnemy == null || (m_CurrentEnemy.transform.position - m_TowerTop.transform.position).sqrMagnitude > m_MaxRange)
        {
            foreach (EnemyController enemy in FindObjectsOfType<EnemyController>())
            {
                float distanceSqr = (enemy.transform.position - m_TowerTop.transform.position).sqrMagnitude;
                if (distanceSqr < shortestDistanceSqr)
                {
                    shortestDistanceSqr = distanceSqr;
                    m_CurrentEnemy = enemy;
                }
            }
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

        if(m_CurrentEnemy && (m_CurrentEnemy.transform.position - m_TowerTop.transform.position).sqrMagnitude <= m_MaxRange)
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
   
        shortestDistanceSqr = float.MaxValue;
        
        //else
        //{
        //    first = true;
        //}
    }
}
