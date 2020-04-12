using Tools;
using UnityEngine;

class FreezeWeapon : MonoBehaviour, IShoot
{
    [SerializeField] private GameObjectScriptablePool m_FreezeBulletScriptablePool;

    /// <summary> Shoots towards a direction.</summary>
    /// <param name="direction"> Direction to shoot at.</param>
    public void Shoot(Vector3 direction)
    {
        GameObject bullet = m_FreezeBulletScriptablePool.Rent(true);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.ResetVelocity();
        bulletComponent.transform.position = transform.position;
        bulletComponent.Shoot(direction);
    }
}
