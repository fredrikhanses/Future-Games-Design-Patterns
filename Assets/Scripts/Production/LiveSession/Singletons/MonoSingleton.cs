using System;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T: MonoSingleton<T>
{
    private static T m_Instance;
    private const string k_DontDestroy = "DontDestroy";

    public static T Instance
    {
        get
        {
            if(m_Instance == null)
            {
                T[] singletons = FindObjectsOfType<T>();
                if(singletons.Length > 1)
                {
                    throw new InvalidOperationException($"There is more than one {typeof(T).Name} instance in the scene");
                }
                if(singletons.Length > 0)
                {
                    m_Instance = singletons[0];
                }
                if(m_Instance == null)
                {
                    object[] customAttributes= typeof(T).GetCustomAttributes(typeof(MonoSecureSingletonAttribute), false);
                    if (customAttributes.Length > 0 && customAttributes[0] is MonoSecureSingletonAttribute attribute)
                    {
                        GameObject gO = new GameObject(typeof(T).Name);
                        m_Instance = gO.AddComponent<T>();
                    }
                    else
                    {
                        object[] singletonConfigs = typeof(T).GetCustomAttributes(typeof(MonoSingletonConfigurationAttribute), false);
                        if (singletonConfigs.Length > 0 && singletonConfigs[0] is MonoSingletonConfigurationAttribute singletonConfig)
                        {
                            string path = singletonConfig.ResourcesPath;
                            GameObject singletonPrefab = Resources.Load<GameObject>(path);
                            if (singletonPrefab == null)
                            {
                                throw new NullReferenceException($"There is no {typeof(T).Name} prefab in the resources folder");
                            }
                            GameObject obj = Instantiate(singletonPrefab);
                            m_Instance = obj.GetComponent<T>();
                            if (m_Instance == null)
                            {
                                throw new NullReferenceException($"There is no {typeof(T).Name} component attached to the singleton prefab");
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException($"The singleton type { typeof(T).Name } doesn't include the mandatory attribute { typeof(MonoSingletonConfigurationAttribute)}");
                        }
                    }
                }
                DontDestroyOnLoad(m_Instance.gameObject);
                m_Instance.gameObject.tag = k_DontDestroy;
            }
            return m_Instance;
        }
    }

    protected virtual void Awake()
    {
        if(m_Instance == null)
        {
            m_Instance = (T)this; //GetComponent<T>();
            DontDestroyOnLoad(gameObject);
            m_Instance.gameObject.tag = k_DontDestroy;
        }
        else if(m_Instance != this)
        {
            throw new InvalidOperationException($"There is more than one {typeof(T).Name} instance in the scene");
        }
    }
}