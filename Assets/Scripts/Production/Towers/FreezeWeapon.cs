using Tools;
using UnityEngine;

class FreezeWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObjectScriptablePool m_FreezeBulletScriptablePool;

    public void Shoot(Vector3 direction)
    {
        GameObject bullet = m_FreezeBulletScriptablePool.Rent(true);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.Reset();
        bulletComponent.transform.position = transform.position;
        bulletComponent.Push(direction);
    }
}
