using System;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthListener : MonoBehaviour
{
    [SerializeField] private Text m_TextField;

    [SerializeField] private Player m_Player;
    private IDisposable m_Subscription;
    private string m_PlayerDied = "Player Died";

    private void OnEnable()
    {
        if (m_Player != null)
        {
            m_Subscription = m_Player.Health.Subscribe(UpdateTextField);
        }
    }

    private void Start()
    {
        if(m_TextField == null)
        {
            m_TextField = GetComponent<Text>();
        }
        if (m_Player != null)
        {
            m_Player = FindObjectOfType<Player>();
            m_Subscription = m_Player.Health.Subscribe(UpdateTextField);
        }
    }

    private void OnDisable()
    {
        m_Subscription?.Dispose();
    }

    private void UpdateTextField(int playerHealth)
    {
        //if (playerHealth <= 0)
        //{
        //    textField.text = m_PlayerDied;
        //}
        //else
        //{
            m_TextField.text = playerHealth.ToString();
        //}
    }
}