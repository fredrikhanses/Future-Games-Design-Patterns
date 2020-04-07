using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private int health = 10;
    [SerializeField] private float speed = 2f;
    [SerializeField] Rigidbody m_Rigidbody;

    private int m_OriginalHealth;
    private float groundHeight = 0.8f;
    private Vector3 targetPosition;
    private bool move;
    private bool stop;
    private LinkedList<Vector3> walkPoints;
    private readonly float maxDistanceToGoal = 0.01f;
    private Vector3 velocity;
    private Animator m_Animator;

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
        m_OriginalHealth = health;
    }

    private void FixedUpdate()
    {
        if(move)
        {
            float distanceSquard = (transform.position - targetPosition).sqrMagnitude;
            if (distanceSquard < maxDistanceToGoal * maxDistanceToGoal)
            {   
                m_Rigidbody.velocity = Vector3.zero;
                transform.position = targetPosition;
                move = false;
                stop = true;
            }
        }
        if(stop)
        {
            stop = false;
            MoveToNext();
        }
    }

    public void MoveStart(LinkedList<Vector3> walkPoints)
    {
        if(walkPoints.Count > 0)
        {      
            this.walkPoints = new LinkedList<Vector3>(walkPoints);
            Debug.Log(this.walkPoints.Count);
            /// <summary>
            ///    Remove spawnPosition
            /// </summary>
            this.walkPoints.RemoveFirst();
            Debug.Log(this.walkPoints.First.Value);
            m_Animator.SetBool("isWalking", true);
            MoveTo(this.walkPoints.First.Value);
            this.walkPoints.RemoveFirst(); 
        }
        else
        {
            Debug.Log($"walkPoints: {walkPoints.Count}");
        }
    }
    
    private void MoveTo(Vector3 targetPosition)
    {
        this.targetPosition = transform.position;
        velocity.x = Pythagoras(this.targetPosition.x, targetPosition.x);
        velocity.z = Pythagoras(this.targetPosition.z, targetPosition.z);
        transform.rotation = Quaternion.LookRotation(velocity);
        this.targetPosition = targetPosition;
        this.targetPosition.y += groundHeight;
        Debug.Log(this.targetPosition);
        Debug.Log(velocity);
        m_Rigidbody.velocity = velocity.normalized * speed;
        move = true;
    }

    private float Pythagoras(float side, float hypotenuse)
    {
        if (side > hypotenuse)
        {
            return -Mathf.Sqrt(Mathf.Abs(hypotenuse * hypotenuse - side * side));
        }
        if(Mathf.Abs(side) > Mathf.Abs(hypotenuse))
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
        if (walkPoints.Count > 0)
        {
            Debug.Log(walkPoints.First.Value);
            MoveTo(walkPoints.First.Value);
            walkPoints.RemoveFirst();
        }
        else
        {
            //DamagePlayer
            Sleep(); 
        }
    }

    public void Reset(Vector3 spawnPosition)
    {
        m_Rigidbody.velocity = Vector3.zero;
        health = m_OriginalHealth;
        m_Animator.SetBool("Killed", false);
        m_Animator.SetBool("isWalking", false);
        m_Animator.SetBool("Damaged", false);
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
        if (other.CompareTag("Bullet") && health > 0)
        {
            health--;
            if (health <= 0)
            {
                m_Animator.SetBool("Killed", true);
                Invoke(nameof(Sleep), 0.5f);
            }
            else
            {
                m_Animator.SetBool("Damaged", true);
            }
        }
    }
}
