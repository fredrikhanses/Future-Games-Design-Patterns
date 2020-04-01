[SecureSingleton]
public class ResourceManager : MonoSingleton<ResourceManager>
{
    public string GetJsonData()
    {
        return "This is my Json data";
    }
}