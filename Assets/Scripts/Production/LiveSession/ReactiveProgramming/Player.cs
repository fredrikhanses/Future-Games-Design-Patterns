using Tools;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ObservableProperty<int> Health = new ObservableProperty<int>();
    public ObservableProperty<string> Name = new ObservableProperty<string>();
   
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
