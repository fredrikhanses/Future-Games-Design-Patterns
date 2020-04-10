using UnityEngine;

public interface IWeapon
{
    void Shoot(Vector3 direction);
}

public class Weapon : MonoBehaviour
{
    private IWeapon m_Weapon;

    private void Start()
    {
        m_Weapon = GetComponent<IWeapon>();
    }

    public void Shoot(Vector3 direction)
    {
        m_Weapon.Shoot(direction);
    }
}
