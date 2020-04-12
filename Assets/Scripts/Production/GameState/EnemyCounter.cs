using System;
using UnityEngine;

public interface IDecreaseEnemyWaves
{
    void DecreaseWaves();
}

public interface IDecreaseStandardEnemies
{
    void DecreaseStandardEnemies();
}

public interface IDecreaseBigEnemies
{
    void DecreaseBigEnemies();
}

public interface IDecreaseActiveEnemies
{
    void DecreaseActiveEnemies();
}

public interface IIncreaseActiveEnemies
{
    void IncreaseActiveEnemies();
}

public interface IWinGame
{
    void WinGame();
}

public interface IEnemyCounter : IDecreaseEnemyWaves, IDecreaseStandardEnemies, 
    IDecreaseBigEnemies, IIncreaseActiveEnemies, IDecreaseActiveEnemies, IWinGame { }

class EnemyCounter : MonoBehaviour, IEnemyCounter
{
    private int m_EnemyWaves;
    private int m_ActiveEnemies;
    private int m_EnemyReinforcements;

    public int StandardEnemies { get; set; }
    public int BigEnemies { get; set; }

    public event Action<int> OnEnemyWavesChanged;
    public event Action<int> OnActiveEnemiesChanged;
    public event Action<int> OnEnemyReinforcementsChanged;

    public int EnemyReinforcements
    {
        get => m_EnemyReinforcements;
        set
        {
            if (m_EnemyReinforcements != value)
            {
                m_EnemyReinforcements = value;
                OnEnemyReinforcementsChanged?.Invoke(m_EnemyReinforcements);
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

    public int EnemyWaves
    {
        get => m_EnemyWaves;
        set
        {
            if (m_EnemyWaves != value)
            {
                m_EnemyWaves = value;
                OnEnemyWavesChanged?.Invoke(m_EnemyWaves);
            }
        }
    }

    /// <summary> 
    ///     Decrease active enemy amount by one.
    /// </summary>
    public void DecreaseActiveEnemies()
    {
        ActiveEnemies--;
    }

    /// <summary> 
    ///     Increase active enemy amount by one.
    /// </summary>
    public void IncreaseActiveEnemies()
    {
        ActiveEnemies++;
    }

    /// <summary> 
    ///     Decrease enemy wave amount by one.
    /// </summary>
    public void DecreaseWaves()
    {
        EnemyWaves--;
    }

    /// <summary> 
    ///     Decrease standard enemy amount by one.
    /// </summary>
    public void DecreaseStandardEnemies()
    {
        StandardEnemies--;
        EnemyReinforcements = StandardEnemies + BigEnemies;
    }

    /// <summary> 
    ///     Decrease big enemy amount by one.
    /// </summary>
    public void DecreaseBigEnemies()
    {
        BigEnemies--;
        EnemyReinforcements = StandardEnemies + BigEnemies;
    }

    /// <summary> 
    ///     Decrease enemy waves and if negative win game.
    /// </summary>
    public void WinGame()
    {
        EnemyWaves--;
    }
}