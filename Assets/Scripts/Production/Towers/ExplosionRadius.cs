using UnityEngine;

public class ExplosionRadius : MonoBehaviour
{
    [SerializeField] private float m_Lifetime = 0.1f;

    private void Sleep()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Invoke(nameof(Sleep), m_Lifetime);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Sleep));
    }
}
