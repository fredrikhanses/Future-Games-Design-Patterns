using UnityEngine;

public interface IShoot
{
    void Shoot(Vector3 direction);
}

public class Weapon : MonoBehaviour
{
    private IShoot m_Weapon;

    private void Start()
    {
        m_Weapon = GetComponent<IShoot>();
    }

    /// <summary> Shoots towards a direction.</summary>
    /// <param name="direction"> Direction to shoot at.</param>
    public void Shoot(Vector3 direction)
    {
        m_Weapon.Shoot(direction);
    }
}
