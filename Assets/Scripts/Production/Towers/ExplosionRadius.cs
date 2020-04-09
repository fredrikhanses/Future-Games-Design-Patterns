using UnityEngine;

public class ExplosionRadius : MonoBehaviour
{
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
        Invoke(nameof(Sleep), 1.0f);
    }
}
