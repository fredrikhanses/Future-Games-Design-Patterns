using UnityEngine;

public interface ITriggerable
{
    void OnTriggerEnter();
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

    private float m_VelocityAdjustment = 2.5f;
    private ITriggerable m_Bullet;

    private void Start()
    {
        m_Bullet = GetComponent<ITriggerable>();
    }

    /// <summary> Shoots towards a direction.</summary>
    /// <param name="direction"> Direction to shoot at.</param>
    public void Shoot(Vector3 direction)
    {
        m_Rigidbody.velocity = direction * m_VelocityAdjustment;
        Invoke(nameof(Sleep), m_Lifetime);
    }

    /// <summary>
    ///     Stops the bullet.
    /// </summary>
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
        m_Bullet.OnTriggerEnter();
        gameObject.SetActive(false);
    }
}
