using UnityEngine;

public interface IBullet
{
    void OnTriggerEnter(Collider other);
}

public class Bullet : MonoBehaviour
{
    [SerializeField] private float m_MinSpeed;
    [SerializeField] private float m_MaxSpeed;
    [SerializeField] private float m_Lifetime = 2f;
    [SerializeField] private Rigidbody m_Rigidbody;

    private IBullet m_Bullet;

    private void Start()
    {
        m_Bullet = GetComponent<IBullet>();
    }

    public void Push(Vector3 direction)
    {
        m_Rigidbody.velocity = direction * Random.Range(m_MinSpeed, m_MaxSpeed);
        Invoke(nameof(Sleep), m_Lifetime);
    }
    
    public void Reset()
    {
        m_Rigidbody.velocity = Vector3.zero;    
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
        m_Bullet.OnTriggerEnter(other);
        Invoke(nameof(Sleep), 0.0f);
    }
}
