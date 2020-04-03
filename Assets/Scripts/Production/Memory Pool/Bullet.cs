using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private Rigidbody m_Rigidbody;

    public void Push()
    {
        m_Rigidbody.velocity = Random.onUnitSphere * Random.Range(minSpeed, maxSpeed);
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
}
