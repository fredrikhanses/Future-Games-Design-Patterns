using Tools;
using UnityEngine;

class BombWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObjectScriptablePool m_BombBulletScriptablePool;

    public void Shoot(Vector3 direction)
    {
        GameObject bullet = m_BombBulletScriptablePool.Rent(true);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.Reset();
        bulletComponent.transform.position = transform.position;
        bulletComponent.Push(direction);
    }
}
