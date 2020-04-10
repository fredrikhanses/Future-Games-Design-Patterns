using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PlayerHealthListener : MonoBehaviour
{
    [SerializeField] private Text m_TextField;
    [SerializeField] private float m_GameOverDelay = 2.0f;
    private Player m_Player;
    private string m_PlayerDied = "Player Died";

    private void OnEnable()
    {
        if (m_Player != null)
        {
            m_Player.OnPlayerHealthChanged += UpdateTextField;
        }
    }

    private void Start()
    {
        if (m_TextField == null)
        {
            m_TextField = GetComponent<Text>();
        }
        m_Player = FindObjectOfType<Player>();
        m_Player.OnPlayerHealthChanged += UpdateTextField;
        m_Player.ResetHealth();
    }

    private void OnDisable()
    {
        m_Player.OnPlayerHealthChanged -= UpdateTextField;
    }

    private void UpdateTextField(int playerHealth)
    {
        if (playerHealth <= 0)
        {
            m_TextField.text = m_PlayerDied;
            Invoke(nameof(GameOverScreen), m_GameOverDelay);
        }
        else
        {
            m_TextField.text = playerHealth.ToString();
        }
        if (playerHealth >= 100)
        {
            Invoke(nameof(WinScreen), m_GameOverDelay);
        }
    }
     
    private void WinScreen()
    {
        m_TextField.text = "PLAYER WINS";
        Time.timeScale = 0f;
    }

    private void GameOverScreen()
    {
        m_TextField.text = "GAME OVER";
        Time.timeScale = 0f;
    }
}