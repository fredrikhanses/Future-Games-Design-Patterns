using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] Rigidbody m_Rigidbody;

    private Vector3 targetPosition;
    private bool move;
    private bool stop;
    private float waitTime = 0.1f;
    private readonly LinkedList<Vector3> walkPoints = new LinkedList<Vector3>();
    private readonly float maxDistanceToGoal = 0.1f;
    private Vector3 velocity;

    private void MoveTo(Vector3 targetPosition)
    {
        this.targetPosition.x = Pythagoras(transform.position.x, targetPosition.x);
        this.targetPosition.y = 1f;
        this.targetPosition.z = Pythagoras(transform.position.z, targetPosition.z);
        velocity.x = targetPosition.x == 0f ? 0f : this.targetPosition.x;
        velocity.z = targetPosition.z == 0f ? 0f : this.targetPosition.z;
        Debug.Log(this.targetPosition);
        Debug.Log(velocity);
        m_Rigidbody.velocity = velocity.normalized * speed; // AddForce(this.targetPosition * speed);
        move = true;
    }

    private float Pythagoras(float side, float hypotenuse)
    {
        //float c2 = hypotenuse * hypotenuse;
        //float a2 = side * side;
        //float b2 = c2 - a2;
        //float oppositeSide; 
        return Mathf.Sqrt(hypotenuse * hypotenuse - side * side);
    }

    public void Move(IEnumerable<Vector2Int> path, List<KeyValuePair<Vector2Int, Vector3>> mapPositions)
    {
        foreach (Vector2Int tilePosition in path)
        {
            foreach (KeyValuePair<Vector2Int, Vector3> mapPosition in mapPositions)
            {
                if (tilePosition.Equals(mapPosition.Key))
                {
                    walkPoints.AddLast(mapPosition.Value);
                }
            }
        }
        Debug.Log(walkPoints.Count);
        //walkPoints.RemoveFirst();
        Debug.Log(walkPoints.First.Value);
        MoveTo(walkPoints.First.Value);
        walkPoints.RemoveFirst();
    }

    private Vector3 CalculateDirection(Vector3 start, Vector3 finish)
    {
        Vector3 direction = start - finish;
        return direction;
    }

    private void Awake()
    {
        if (m_Rigidbody == null)
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }
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
            //waitTime -= Time.fixedDeltaTime;
            //if(waitTime <= 0)
            //{
            //    waitTime = 0.1f;
                stop = false;
                MoveToNext();
            //}
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
    }

    //Lerp
    //// Transforms to act as start and end markers for the journey.
    //public Transform startMarker;
    //public Transform endMarker;

    //// Movement speed in units per second.
    //public float speed = 1.0F;

    //// Time when the movement started.
    //private float startTime;

    //// Total distance between the markers.
    //private float journeyLength;

    //void Start()
    //{
    //    // Keep a note of the time the movement started.
    //    startTime = Time.time;

    //    // Calculate the journey length.
    //    journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    //}

    //// Move to the target end position.
    //void Update()
    //{
    //    // Distance moved equals elapsed time times speed..
    //    float distCovered = (Time.time - startTime) * speed;

    //    // Fraction of journey completed equals current distance divided by total distance.
    //    float fractionOfJourney = distCovered / journeyLength;

    //    // Set our position as a fraction of the distance between the markers.
    //    transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
    //}
}
