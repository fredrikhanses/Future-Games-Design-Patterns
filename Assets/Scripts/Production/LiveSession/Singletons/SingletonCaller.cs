using UnityEngine;

public class SingletonCaller : MonoBehaviour
{
    [ContextMenu("Create Enemy")]
    public void CreateEnemy()
    {
        
    }

    [ContextMenu("Load Json File")]
    public void LoadJsonFile()
    {
        string data = ResourceManager.Instance.GetJsonData();
        Debug.Log(data);
    }

    [ContextMenu("Call POCO singleton")]
    public void Call()
    {
        
    }

}