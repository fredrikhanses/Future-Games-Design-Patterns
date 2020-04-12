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
    public event Action<int> OnPlayerHealthChanged;

    private bool m_PauseGame = false;
    private int m_Health;
    private float m_OriginalTimeScale;
    
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

    private void Awake()
    {
        m_OriginalTimeScale = Time.timeScale;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_PauseGame = !m_PauseGame;
            if (m_PauseGame)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = m_OriginalTimeScale;
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