﻿using System;
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

    public void Damage(int damage)
    {
        if (Health > 0)
        {
            Health -= damage;
        }
    }
}