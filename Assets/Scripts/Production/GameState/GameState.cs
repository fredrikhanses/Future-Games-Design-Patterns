using System;
using UnityEngine;

class GameState : MonoBehaviour
{
    private int m_WaveNumber;
    private int m_NormalEnemies;
    private int m_StrongEnemies;

    public event Action<int> OnWaveNumberChanged;
    public event Action<int> OnNormalEnemiesChanged;
    public event Action<int> OnStrongEnemiesChanged;

    public int WaveNumber
    {
        get => m_WaveNumber;
        set
        {
            if (m_WaveNumber != value)
            {
                m_WaveNumber = value;
                OnWaveNumberChanged?.Invoke(m_WaveNumber);
            }
        }
    }

    public int NormalEnemies
    {
        get => m_NormalEnemies;
        set
        {
            if (m_NormalEnemies != value)
            {
                m_NormalEnemies = value;
                OnNormalEnemiesChanged?.Invoke(m_NormalEnemies);
            }
        }
    }

    public int StrongEnemies
    {
        get => m_StrongEnemies;
        set
        {
            if (m_StrongEnemies != value)
            {
                m_StrongEnemies = value;
                OnStrongEnemiesChanged?.Invoke(m_StrongEnemies);
            }
        }
    }

    public void DecreaseWaveNumber()
    {
        WaveNumber--;
    }

    public void DecreaseNormalEnemies()
    {
        NormalEnemies--;
    }

    public void DecreaseStrongEnemies()
    {
        StrongEnemies--;
    }

    public void WinGame()
    {
        WaveNumber--;
    }
}

