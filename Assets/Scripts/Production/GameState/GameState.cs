using System;
using UnityEngine;

public interface IDecreaseEnemyWaves
{
    void DecreaseWaves();
}

public interface IDecreaseNormalEnemies
{
    void DecreaseNormalEnemies();
}

public interface IDecreaseStrongEnemies
{
    void DecreaseStrongEnemies();
}

public interface IEnemyCounter : IDecreaseEnemyWaves, IDecreaseNormalEnemies, IDecreaseStrongEnemies { }

class GameState : MonoBehaviour, IEnemyCounter
{
    private int m_WaveNumber;
    private int m_NormalEnemies;
    private int m_StrongEnemies;
    private int m_ActiveEnemies;
    private int m_EnemyReinforcement;

    public event Action<int> OnWaveNumberChanged;
    public event Action<int> OnNormalEnemiesChanged;
    public event Action<int> OnStrongEnemiesChanged;
    public event Action<int> OnActiveEnemiesChanged;
    public event Action<int> OnEnemyReinforcementChanged;

    public int EnemyReinforcement
    {
        get => m_EnemyReinforcement;
        set
        {
            if (m_EnemyReinforcement != value)
            {
                m_EnemyReinforcement = value;
                OnEnemyReinforcementChanged?.Invoke(m_EnemyReinforcement);
            }
        }
    }

    public int ActiveEnemies
    {
        get => m_ActiveEnemies;
        set
        {
            if (m_ActiveEnemies != value)
            {
                m_ActiveEnemies = value;
                OnActiveEnemiesChanged?.Invoke(m_ActiveEnemies);
            }
        }
    }

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

    public void DecreaseActiveEnemies()
    {
        ActiveEnemies--;
    }

    public void IncreaseActiveEnemies()
    {
        ActiveEnemies++;
    }


    public void DecreaseWaves()
    {
        WaveNumber--;
    }

    public void DecreaseNormalEnemies()
    {
        NormalEnemies--;
        EnemyReinforcement = NormalEnemies + StrongEnemies;
    }

    public void DecreaseStrongEnemies()
    {
        StrongEnemies--;
        EnemyReinforcement = NormalEnemies + StrongEnemies;
    }

    public void WinGame()
    {
        WaveNumber--;
    }
}

