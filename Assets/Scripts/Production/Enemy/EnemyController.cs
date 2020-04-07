using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] Rigidbody m_Rigidbody;

    private float groundHeight = 0.8f;
    private Vector3 targetPosition;
    private bool move;
    private bool stop;
    private LinkedList<Vector3> walkPoints;
    private readonly float maxDistanceToGoal = 0.01f;
    private Vector3 velocity;

    private void Awake()
    {
        if (m_Rigidbody == null)
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }
        targetPosition = transform.position;
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
            Destroy(gameObject);  
        }
    }
}
