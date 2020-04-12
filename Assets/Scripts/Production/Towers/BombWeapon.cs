using Tools;
using UnityEngine;

class BombWeapon : MonoBehaviour, IShoot
{
    [SerializeField] private GameObjectScriptablePool m_BombBulletScriptablePool;

    /// <summary> Shoots towards a direction.</summary>
    /// <param name="direction"> Direction to shoot at.</param>
    public void Shoot(Vector3 direction)
    {
        GameObject bullet = m_BombBulletScriptablePool.Rent(true);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.ResetVelocity();
        bulletComponent.transform.position = transform.position;
        bulletComponent.Shoot(direction);
    }
}
