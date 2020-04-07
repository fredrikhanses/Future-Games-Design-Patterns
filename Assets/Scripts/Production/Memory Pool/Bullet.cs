using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private Rigidbody m_Rigidbody;
    [SerializeField] private GameObject m_Explosion;

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
        //ObjecPool this
        Instantiate(m_Explosion, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
