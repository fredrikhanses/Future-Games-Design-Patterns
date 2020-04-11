using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GameStateListener : MonoBehaviour
{
    [SerializeField] private Text m_EnemyWaveTextField;
    [SerializeField] private Text m_NormalEnemiesTextField;
    [SerializeField] private Text m_StrongEnemiesTextField;
    [SerializeField] private Text m_GameStateTextField;
    [SerializeField] private Text m_GameTimeTextField;
    [SerializeField] private float m_GameOverDelay = 2.0f;
    private GameState m_GameState;
    private Player m_Player;

    private void Start()
    {
        m_Player = FindObjectOfType<Player>();
        m_GameState = FindObjectOfType<GameState>();
        m_GameState.OnWaveNumberChanged += UpdateEnemyWaveTextField;
        m_GameState.OnNormalEnemiesChanged += UpdateNormalEnemiesTextField;
        m_GameState.OnStrongEnemiesChanged += UpdateStrongEnemiesTextField;
    }

    private void OnEnable()
    {
        if (m_GameState != null)
        {
            m_GameState.OnWaveNumberChanged += UpdateEnemyWaveTextField;
            m_GameState.OnNormalEnemiesChanged += UpdateNormalEnemiesTextField;
            m_GameState.OnStrongEnemiesChanged += UpdateStrongEnemiesTextField;
        }
    }

    private void OnDisable()
    {
        m_GameState.OnWaveNumberChanged -= UpdateEnemyWaveTextField;
        m_GameState.OnNormalEnemiesChanged -= UpdateNormalEnemiesTextField;
        m_GameState.OnStrongEnemiesChanged -= UpdateStrongEnemiesTextField;
    }

    private void UpdateEnemyWaveTextField(int wavesRemaining)
    {
        if (wavesRemaining < 0)
        {
            m_EnemyWaveTextField.text = "0";
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
            m_NormalEnemiesTextField.text = "0";
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
            m_StrongEnemiesTextField.text = "0";
        }
        else
        {
            m_StrongEnemiesTextField.text = strongEnemiesRemaining.ToString();
        }
    }

    private void WinGame()
    {
        if (m_GameState.NormalEnemies <= 0 && m_GameState.StrongEnemies <= 0 && m_GameState.WaveNumber <= 0)
        {
            bool winGame = false;
            foreach (EnemyController enemy in EnemyManager.Instance.EnemyControllers)
            {
                if (enemy.isActiveAndEnabled)
                {
                    winGame = false;
                    break;
                }
                else
                {
                    winGame = true;
                }
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
        m_EnemyWaveTextField.text = null;
        m_NormalEnemiesTextField.text = null;
        m_StrongEnemiesTextField.text = null;
        m_GameTimeTextField.text = Time.realtimeSinceStartup.ToString();
        m_GameStateTextField.text = "PLAYER WINS";
        Time.timeScale = 0f;
    }

    private void GameOverScreen()
    {
        m_GameTimeTextField.text = Time.realtimeSinceStartup.ToString();
        m_GameStateTextField.text = "GAME OVER";
        Time.timeScale = 0f;
    }
}