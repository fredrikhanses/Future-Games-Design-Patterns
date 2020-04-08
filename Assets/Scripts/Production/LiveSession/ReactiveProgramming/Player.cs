using Tools;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int m_InitHealth;
    public ObservableProperty<int> Health { get; } = new ObservableProperty<int>();
    public ObservableProperty<string> Name { get; } = new ObservableProperty<string>();

    private void Start()
    {
        Health.Value = m_InitHealth;    
    }

    [ContextMenu("Increase Health")]
    public void IncreaseHealth()
    {
        Health.Value++;
    }

    [ContextMenu("Decrease Health")]
    public void DecreaseHealth()
    {
        Health.Value--;
    }
}
