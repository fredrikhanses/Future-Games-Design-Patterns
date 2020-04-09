using Tools;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private Rigidbody m_Rigidbody;
    [SerializeField] private GameObjectScriptablePool m_ExplosionScriptablePool;
    [SerializeField] private GameObjectScriptablePool m_ExplosionRadiusScriptablePool;

    public void Push(Vector3 direction)
    {
        m_Rigidbody.velocity = direction * Random.Range(minSpeed, maxSpeed);
        Invoke(nameof(Sleep), 2f);
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
        if (m_ExplosionScriptablePool != null && m_ExplosionRadiusScriptablePool != null)
        {
            m_ExplosionScriptablePool.Rent(true).transform.position = transform.position;
            m_ExplosionRadiusScriptablePool.Rent(true).transform.position = transform.position;
        }
        Invoke(nameof(Sleep), 0.0f);
    }
}
