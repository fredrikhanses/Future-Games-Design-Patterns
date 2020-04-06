using System;
using UnityEngine;

public class Player : MonoBehaviour
{   
    [SerializeField] private int health;
    public event Action<int> OnPlayerHealthChanged;
    public int Health
    {
        get => health;
        set
        {
            if(health != value)
            {
                health = value;
                OnPlayerHealthChanged?.Invoke(health);
            }
        }
    }

    [SerializeField] private string name;
    public event Action<string> OnNameChanged;
    public string Name
    {
        get => name;
        set
        {
            if (name != value)
            {
                name = value;
                OnNameChanged?.Invoke(name);
            }
        }
    }

    private void Start()
    {
        OnPlayerHealthChanged?.Invoke(health);
    }

    [ContextMenu("Increase Health")]
    public void IncreaseHealth()
    {
        Health++;
    }

    [ContextMenu("Decrease Health")]
    public void DecreaseHealth()
    {
        Health--;
    }
}
