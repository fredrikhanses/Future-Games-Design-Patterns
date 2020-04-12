using System;
using UnityEngine;

public interface IResetHealth
{
    void ResetHealth();
}

public interface ITakeDamage
{
    void TakeDamage(int damage);
}

public interface IPlayer : ITakeDamage, IResetHealth { }

public class Player : MonoBehaviour
{
    [SerializeField] private int m_InitHealth;
    private int m_Health;
    public event Action<int> OnPlayerHealthChanged;

    public int Health
    {
        get => m_Health;
        set
        {
            if (m_Health != value)
            {
                m_Health = value;
                OnPlayerHealthChanged?.Invoke(m_Health);
            }
        }
    }

    public void ResetHealth()
    {
        Health = m_InitHealth;
    }

    public void TakeDamage(int damage)
    {
        if (Health > 0)
        {
            Health -= damage;
        }
    }
}