using System;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Text m_GameStateTextField;
    public event Action<int> OnPlayerHealthChanged;

    private const string k_GamePaused = "GAME PAUSED";
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
                m_GameStateTextField.text = k_GamePaused;
            }
            else
            {
                m_GameStateTextField.text = null;
                Time.timeScale = m_OriginalTimeScale;
            }
        }
    }

    /// <summary> 
    ///     Decrease active enemy amount by one.
    /// </summary>
    public void ResetHealth()
    {
        Health = m_InitHealth;
    }

    /// <summary> Inflict damage.</summary>
    /// <param name="damage"> Amount of damage.</param>
    public void TakeDamage(int damage)
    {
        if (Health > 0)
        {
            Health -= damage;
        }
    }
}