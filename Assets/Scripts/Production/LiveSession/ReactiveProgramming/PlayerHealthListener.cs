//using System;
//using Tools;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PlayerHealthListener : MonoBehaviour
{
    [SerializeField] private Text m_TextField;
    private Player m_Player;
    private string m_PlayerDied = "Player Died";

    private void Awake()
    {
    }

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
            Invoke(nameof(GameOverScreen), 2f);
        }
        else
        {
            m_TextField.text = playerHealth.ToString();
        }
    }

    private void GameOverScreen()
    {
        m_TextField.text = "GAME OVER";
    }
}

//public class PlayerHealthListener : MonoBehaviour
//{
//    [SerializeField] private Text m_TextField;

//    [SerializeField] private Player m_Player;
//    private IDisposable m_Subscription;
//    private string m_PlayerDied = "Player Died";

//    private void OnEnable()
//    {
//        if (m_Player != null)
//        {
//            m_Subscription = m_Player.Health.Subscribe(UpdateTextField);
//        }
//    }

//    private void Start()
//    {
//        if(m_TextField == null)
//        {
//            m_TextField = GetComponent<Text>();
//        }
//        if (m_Player != null)
//        {
//            m_Player = FindObjectOfType<Player>();
//            m_Subscription = m_Player.Health.Subscribe(UpdateTextField);
//        }
//    }

//    private void OnDisable()
//    {
//        m_Subscription?.Dispose();
//    }

//    private void UpdateTextField(int playerHealth)
//    {
//        //if (playerHealth <= 0)
//        //{
//        //    m_TextField.text = m_PlayerDied;
//        //}
//        //else
//        //{
//            m_TextField.text = playerHealth.ToString();
//        //}
//    }
//}