using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PlayerHealthListener : MonoBehaviour
{
    [SerializeField] private Text m_TextField;
    [SerializeField] private EnemyCounterListener m_EnemyCounterListener;
    private Player m_Player;
    private const string k_PlayerDied = "Player Died";

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
        if (m_EnemyCounterListener == null)
        {
            m_EnemyCounterListener = GetComponent<EnemyCounterListener>();
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
            m_TextField.text = k_PlayerDied;
            m_EnemyCounterListener.LoseGame();
        }
        else
        {
            m_TextField.text = playerHealth.ToString();
        }
    }
}