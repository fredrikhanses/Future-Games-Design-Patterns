using System;
using UnityEngine;

public class Player : MonoBehaviour
{   
    public event Action<int> OnPlayerHealthChanged;

    [SerializeField] private int health;
    
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
