using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthListener : MonoBehaviour
{
    [SerializeField]
    private Text textField;

    private string playerDied = "Player Died";
    private Player player;

    private void OnEnable()
    {
        if (player != null)
        {
            player.OnPlayerHealthChanged += UpdateTextField;
        }
    }

    private void Start()
    {
        if(textField == null)
        {
            textField = GetComponent<Text>();
        }
        player = FindObjectOfType<Player>();
        player.OnPlayerHealthChanged += UpdateTextField;
    }

    private void OnDisable()
    {
        player.OnPlayerHealthChanged -= UpdateTextField;
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
