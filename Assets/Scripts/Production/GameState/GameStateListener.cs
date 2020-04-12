using UnityEngine;
using UnityEngine.UI;

public interface ILoseGame
{
    void LoseGame();
}

[RequireComponent(typeof(Text))]
public class GameStateListener : MonoBehaviour, ILoseGame
{
    [SerializeField] private Text m_EnemyWaveTextField;
    [SerializeField] private Text m_NormalEnemiesTextField;
    [SerializeField] private Text m_StrongEnemiesTextField;
    [SerializeField] private Text m_GameStateTextField;
    [SerializeField] private Text m_GameTimeTextField;
    [SerializeField] private Text m_EnemyReinforcementTextField;
    [SerializeField] private GameObject m_GameTimeText;
    [SerializeField] private Text m_ActiveEnemiesTextField;
    [SerializeField] private float m_GameOverDelay = 2.0f;
    [SerializeField] private GameState m_GameState;
    [SerializeField] private Player m_Player;

    private const string k_Zero = "0";
    private const string k_PlayerWins = "PLAYER WINS";
    private const string k_GameOver = "GAME OVER";

    private void OnEnable()
    {
        if (m_GameState != null)
        {
            m_GameState.OnWaveNumberChanged += UpdateEnemyWaveTextField;
            m_GameState.OnNormalEnemiesChanged += UpdateNormalEnemiesTextField;
            m_GameState.OnStrongEnemiesChanged += UpdateStrongEnemiesTextField;
            m_GameState.OnActiveEnemiesChanged += UpdateActiveEnemiesTextField;
            m_GameState.OnEnemyReinforcementChanged += UpdateEnemyReinforcementTextField;
        }
    }

    private void Start()
    {
        if (m_Player == null)
        {
            m_Player = FindObjectOfType<Player>();

        }
        if (m_GameState == null)
        {
            m_GameState = FindObjectOfType<GameState>();
        }
        m_GameState.OnWaveNumberChanged += UpdateEnemyWaveTextField;
        m_GameState.OnNormalEnemiesChanged += UpdateNormalEnemiesTextField;
        m_GameState.OnStrongEnemiesChanged += UpdateStrongEnemiesTextField;
        m_GameState.OnActiveEnemiesChanged += UpdateActiveEnemiesTextField;
        m_GameState.OnEnemyReinforcementChanged += UpdateEnemyReinforcementTextField;
    }

    private void OnDisable()
    {
        m_GameState.OnWaveNumberChanged -= UpdateEnemyWaveTextField;
        m_GameState.OnNormalEnemiesChanged -= UpdateNormalEnemiesTextField;
        m_GameState.OnStrongEnemiesChanged -= UpdateStrongEnemiesTextField;
        m_GameState.OnActiveEnemiesChanged -= UpdateActiveEnemiesTextField;
        m_GameState.OnEnemyReinforcementChanged -= UpdateEnemyReinforcementTextField;
    }

    private void UpdateEnemyReinforcementTextField(int enemyReinforcement)
    {
        if (enemyReinforcement <= 0)
        {
            m_EnemyReinforcementTextField.text = k_Zero;

        }
        else
        {
            m_EnemyReinforcementTextField.text = enemyReinforcement.ToString();
        }
    }

    private void UpdateEnemyWaveTextField(int wavesRemaining)
    {
        if (wavesRemaining < 0)
        {
            m_EnemyWaveTextField.text = k_Zero;
            WinGame();
        }
        else
        {
            m_EnemyWaveTextField.text = wavesRemaining.ToString();
        }
    }

    private void UpdateNormalEnemiesTextField(int normalEnemiesRemaining)
    {
        if (normalEnemiesRemaining <= 0)
        {
            m_NormalEnemiesTextField.text = k_Zero;
        }
        else
        {
            m_NormalEnemiesTextField.text = normalEnemiesRemaining.ToString();
        }
    }

    private void UpdateStrongEnemiesTextField(int strongEnemiesRemaining)
    {
        if (strongEnemiesRemaining <= 0)
        {
            m_StrongEnemiesTextField.text = k_Zero;
        }
        else
        {
            m_StrongEnemiesTextField.text = strongEnemiesRemaining.ToString();
        }
    }

    private void UpdateActiveEnemiesTextField(int activeEnemies)
    {
        if (activeEnemies <= 0)
        {
            m_ActiveEnemiesTextField.text = k_Zero;
        }
        else
        {
            m_ActiveEnemiesTextField.text = activeEnemies.ToString();
        }
    }

    private void WinGame()
    {
        if (m_GameState.NormalEnemies <= 0 && m_GameState.StrongEnemies <= 0 && m_GameState.WaveNumber <= 0)
        {
            bool winGame = false;
            if (EnemyManager.Instance.ActiveEnemyControllers.Count <= 0)
            {
                winGame = true;
            }
            if (winGame)
            {
                Invoke(nameof(WinScreen), m_GameOverDelay);
            }
        }
    }

    public void LoseGame()
    {
        if (m_Player.Health <= 0)
        {
            Invoke(nameof(GameOverScreen), m_GameOverDelay);
        }
    }

    private void WinScreen()
    {
        m_GameTimeText.SetActive(true);
        m_GameTimeTextField.text = Mathf.RoundToInt(Time.realtimeSinceStartup).ToString();
        m_GameStateTextField.text = k_PlayerWins;
        Time.timeScale = 0f;
    }

    private void GameOverScreen()
    {
        m_GameTimeText.SetActive(true);
        m_GameTimeTextField.text = Mathf.RoundToInt(Time.realtimeSinceStartup).ToString();
        m_GameStateTextField.text = k_GameOver;
        Time.timeScale = 0f;
    }
}