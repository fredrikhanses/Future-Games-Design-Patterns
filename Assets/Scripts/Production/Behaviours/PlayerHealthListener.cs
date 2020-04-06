using System;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthListener : MonoBehaviour
{
    [SerializeField] private Text textField;

    private Player player;
    private IDisposable subscription;
    private string playerDied = "Player Died";

    private void OnEnable()
    {
        if (player != null)
        {
            subscription = player.Health.Subscribe(UpdateTextField);
        }
    }

    private void Start()
    {
        if(textField == null)
        {
            textField = GetComponent<Text>();
        }
        player = FindObjectOfType<Player>();
        if (player != null)
        {
            subscription = player.Health.Subscribe(UpdateTextField);
        }
    }

    private void OnDisable()
    {
        subscription.Dispose();
    }

    private void UpdateTextField(int playerHealth)
    {
        if (playerHealth <= 0)
        {
            textField.text = playerDied;
        }
        else
        {
            textField.text = playerHealth.ToString();
        }
    }
}