using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
[SelectionBase]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private int m_Health;
    [SerializeField] private int m_Damage;
    [SerializeField] private float m_Speed;
    [SerializeField] private Rigidbody m_Rigidbody;
    [SerializeField] private Player m_Player;
    [SerializeField] private Animator m_Animator;

    private bool m_Move;
    private bool m_Stop;
    private int m_InitialHealth;
    private Vector3 m_TargetPosition;
    private Vector3 m_Direction;
    private LinkedList<Vector3> m_Path;
    private const float k_MinDistanceToGoal = 0.01f;
    private const float k_GroundHeight = 0.8f;
    private const float k_KillDelay = 0.5f;
    private const string k_Killed = "Killed";
    private const string k_IsWalking = "isWalking";
    private const string k_Damaged = "Damaged";
    private const string k_Bullet = "Bullet";

    private void Awake()
    {
        if (m_Rigidbody == null)
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }
        if(m_Animator == null)
        {
            m_Animator = GetComponent<Animator>();
        }
        if (m_Player == null)
        {
            m_Player = FindObjectOfType<Player>();
        }
        m_InitialHealth = m_Health;
    }

    private void FixedUpdate()
    {
        if(m_Move)
        {
            float distanceSquard = (transform.position - m_TargetPosition).sqrMagnitude;
            if (distanceSquard < k_MinDistanceToGoal * k_MinDistanceToGoal)
            {   
                m_Rigidbody.velocity = Vector3.zero;
                transform.position = m_TargetPosition;
                m_Move = false;
                m_Stop = true;
            }
        }
        if(m_Stop)
        {
            m_Stop = false;
            MoveToNext();
        }
    }

    /// <summary> Start enemy movement along path.</summary>
    /// <param name="path">Path to walk along.</param>
    public void MoveStart(LinkedList<Vector3> path)
    {
        if(path.Count > 0)
        {      
            m_Path = new LinkedList<Vector3>(path);
            //Remove spawnPosition.
            m_Path.RemoveFirst(); 
            MoveTo(m_Path.First.Value);
            m_Path.RemoveFirst(); 
        }
    }
    
    private void MoveTo(Vector3 targetPosition)
    {
        m_TargetPosition = transform.position;
        m_Direction.x = Pythagoras(m_TargetPosition.x, targetPosition.x);
        m_Direction.z = Pythagoras(m_TargetPosition.z, targetPosition.z);
        transform.rotation = Quaternion.LookRotation(m_Direction);
        m_TargetPosition = targetPosition;
        m_TargetPosition.y += k_GroundHeight;
        m_Rigidbody.velocity = m_Direction.normalized * m_Speed;
        m_Move = true;
    }

    //Calculate direction.
    private float Pythagoras(float side, float hypotenuse)
    {
        if (side > hypotenuse)
        {
            return -Mathf.Sqrt(Mathf.Abs(hypotenuse * hypotenuse - side * side));
        }
        else if(Mathf.Abs(side) > Mathf.Abs(hypotenuse))
        {
            return Mathf.Sqrt(Mathf.Abs(hypotenuse * hypotenuse - side * side));
        }
        else if(side == hypotenuse)
        {
            return 0f;
        }
        else
        {
            return Mathf.Sqrt(hypotenuse * hypotenuse - side * side);
        }
    }

    private void MoveToNext()
    {
        if (m_Path.Count > 0)
        {
            MoveTo(m_Path.First.Value);
            m_Path.RemoveFirst();
        }
        else
        {
            m_Player.Damage(m_Damage);
            Sleep(); 
        }
    }

    /// <summary> Reset enemy to initial state.</summary>
    /// <param name="spawnPosition">Position to spawn at.</param>
    public void Reset(Vector3 spawnPosition)
    {
        m_Rigidbody.velocity = Vector3.zero;
        m_Health = m_InitialHealth;
        m_Animator.SetBool(k_Killed, false);
        m_Animator.SetBool(k_IsWalking, true);
        m_Animator.SetBool(k_Damaged, false);
        transform.position = spawnPosition;
        transform.rotation = Quaternion.identity;
    }

    private void Sleep()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Sleep));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(k_Bullet) && m_Health > 0)
        {
            m_Health--;
            if (m_Health <= 0)
            {
                m_Animator.SetBool(k_Killed, true);
                Invoke(nameof(Sleep), k_KillDelay);
            }
            else
            {
                m_Animator.SetBool(k_Damaged, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(k_Bullet) && m_Health > 0)
        {
            m_Health--;
            if (m_Health <= 0)
            {
                m_Animator.SetBool(k_Killed, true);
                Invoke(nameof(Sleep), k_KillDelay);
            }
            else
            {
                m_Animator.SetBool(k_Damaged, true);
            }
        }
    }
}
