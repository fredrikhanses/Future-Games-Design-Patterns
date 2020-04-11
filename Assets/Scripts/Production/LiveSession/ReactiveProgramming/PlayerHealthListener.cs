using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PlayerHealthListener : MonoBehaviour
{
    [SerializeField] private Text m_TextField;
    [SerializeField] private GameStateListener m_GameStateListener;
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
        if (m_GameStateListener == null)
        {
            m_GameStateListener = GetComponent<GameStateListener>();
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
            m_GameStateListener.LoseGame();
        }
        else
        {
            m_TextField.text = playerHealth.ToString();
        }
    }
}