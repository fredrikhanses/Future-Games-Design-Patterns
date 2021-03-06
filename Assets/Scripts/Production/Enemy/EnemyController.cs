﻿using System.Collections.Generic;
using UnityEngine;

public interface IStartMoving
{
    void StartMoving(IEnumerable<Vector3> path);
}

public interface IReset
{
    void Reset();
}

public interface IResetPosition
{
    void ResetPosition(Vector3 position);
}

public interface IEnemy : IStartMoving, IReset, IResetPosition { }

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
[SelectionBase]
public class EnemyController : MonoBehaviour, IEnemy
{
    [SerializeField] private int m_Health;
    [SerializeField] private int m_Damage;
    [SerializeField] private float m_Speed;
    [SerializeField] private Rigidbody m_Rigidbody;
    [SerializeField] private Player m_Player;
    [SerializeField] private EnemyCounter m_EnemyCounter;
    [SerializeField] private Animator m_Animator;

    private bool m_Move;
    private bool m_Stop;
    private int m_InitialHealth;
    private float m_InitialSpeed;
    private Vector3 m_TargetPosition;
    private Vector3 m_Direction;
    private LinkedList<Vector3> m_Path;
    private const float k_MinDistanceToGoal = 0.01f;
    private const float k_GroundHeight = 0.8f;
    private const float k_KillDelay = 0.5f;
    private const float k_SlowPercent = 0.5f;
    private const float k_SpeedResetDelay = 2f;
    private const string k_Killed = "Killed";
    private const string k_IsWalking = "isWalking";
    private const string k_Damaged = "Damaged";
    private const string k_Bullet = "Bullet";
    private const string k_Freeze = "Freeze";
    private const string k_Explosion = "Explosion";

    private void Awake()
    {
        m_InitialHealth = m_Health;
        m_InitialSpeed = m_Speed;
    }

    private void Start()
    {
        if (m_Rigidbody == null)
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }
        if (m_Animator == null)
        {
            m_Animator = GetComponent<Animator>();
        }
        if (m_Player == null)
        {
            m_Player = FindObjectOfType<Player>();
        }
        if (m_EnemyCounter == null)
        {
            m_EnemyCounter = FindObjectOfType<EnemyCounter>();
        }
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
    /// <param name="path"> Path to walk along.</param>
    public void StartMoving(IEnumerable<Vector3> path)
    {
        m_Path = new LinkedList<Vector3>(path);
        if(m_Path.Count > 0)
        {
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
            m_Player.TakeDamage(m_Damage);
            Sleep(); 
        }
    }

    /// <summary> 
    ///     Reset enemy to initial state.
    /// </summary>
    public void Reset()
    {
        ResetAnimations();
        ResetHealth();
        ResetSpeed();
        ResetVelocity();
    }

    private void ResetAnimations()
    {
        m_Animator.SetBool(k_Killed, false);
        m_Animator.SetBool(k_IsWalking, true);
        m_Animator.SetBool(k_Damaged, false);
    }

    private void ResetHealth()
    {
        m_Health = m_InitialHealth;
    }

    /// <summary> 
    ///     Reset enemy position to spawn position.
    /// </summary>
    public void ResetPosition(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
        transform.rotation = Quaternion.identity;
    }

    private void ResetSpeed()
    {
        m_Speed = m_InitialSpeed;
    }

    private void ResetVelocity()
    {
        m_Rigidbody.velocity = Vector3.zero;
    }

    private void Sleep()
    {
        m_EnemyCounter.DecreaseActiveEnemies();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Sleep));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_Health > 0 && (other.CompareTag(k_Bullet) || other.CompareTag(k_Freeze) || other.CompareTag(k_Explosion)))
        {
            m_Health--;
            if (m_Health <= 0)
            {
                EnemyManager.Instance.ActiveEnemyControllers.Remove(this);
                m_Animator.SetBool(k_Killed, true);
                Invoke(nameof(Sleep), k_KillDelay);
            }
            else
            {
                m_Animator.SetBool(k_Damaged, true);
            }
            if (other.CompareTag(k_Freeze))
            {
                m_Speed *= k_SlowPercent;
                m_Speed = Mathf.Clamp(m_Speed, m_InitialSpeed * k_SlowPercent, m_InitialSpeed);
                Invoke(nameof(ResetSpeed), k_SpeedResetDelay);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_Health > 0 && other.CompareTag(k_Explosion))
        {
            m_Health--;
            if (m_Health <= 0)
            {
                EnemyManager.Instance.ActiveEnemyControllers.Remove(this);
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
