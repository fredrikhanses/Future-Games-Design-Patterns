using System;
//using Tools;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int m_InitHealth;
    private int m_Health;
    public event Action<int> OnPlayerHealthChanged;

    public int Health
    {
        get => m_Health;
        set
        {
            if (m_Health != value)
            {
                m_Health = value;
                OnPlayerHealthChanged?.Invoke(m_Health);
            }
        }
    }

    private string m_Name;
    public event Action<string> OnNameChanged;

    public string Name
    {
        get => m_Name;
        set
        {
            if (m_Name != value)
            {
                m_Name = value;
                OnNameChanged?.Invoke(m_Name);
            }
        }
    }

    private void Start()
    {
        Health = m_InitHealth;
    }

    [ContextMenu("Reset Health")]
    public void ResetHealth()
    {
        Health = m_InitHealth;
    }

    [ContextMenu("Increase Health")]
    public void IncreaseHealth()
    {
        Health++;
    }

    [ContextMenu("Decrease Health")]
    public void DecreaseHealth()
    {
        if (Health > 0)
        {
            Health--;
        } 
    }
}

//public class Player : MonoBehaviour
//{
//    [SerializeField] private int m_InitHealth;
//    public ObservableProperty<int> Health { get; } = new ObservableProperty<int>();
//    public ObservableProperty<string> Name { get; } = new ObservableProperty<string>();

//    private void Start()
//    {
//        Health.Value = m_InitHealth;    
//    }

//    [ContextMenu("Increase Health")]
//    public void IncreaseHealth()
//    {
//        Health.Value++;
//    }

//    [ContextMenu("Decrease Health")]
//    public void DecreaseHealth()
//    {
//        Health.Value--;
//    }
//}
