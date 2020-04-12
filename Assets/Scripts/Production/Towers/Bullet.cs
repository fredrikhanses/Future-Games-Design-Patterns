using UnityEngine;

public interface ITriggerable
{
    void OnTriggerEnter(Collider other);
}

public interface IResetVelocity
{
    void ResetVelocity();
}

public interface IBullet : IShoot, IResetVelocity { }

public class Bullet : MonoBehaviour, IBullet
{
    [SerializeField] private float m_MinSpeed;
    [SerializeField] private float m_MaxSpeed;
    [SerializeField] private float m_Lifetime = 2f;
    [SerializeField] private Rigidbody m_Rigidbody;

    private ITriggerable m_Bullet;

    private void Start()
    {
        m_Bullet = GetComponent<ITriggerable>();
    }

    public void Shoot(Vector3 direction)
    {
        m_Rigidbody.velocity = direction * 2.5f;//Random.Range(m_MinSpeed, m_MaxSpeed);
        Invoke(nameof(Sleep), m_Lifetime);
    }
    
    public void ResetVelocity()
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
        gameObject.SetActive(false);
    }
}
