using System;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T: MonoSingleton<T>
{
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if(m_Instance == null)
            {
                T[] managers = FindObjectsOfType<T>();
                if(managers.Length > 1)
                {
                    throw new InvalidOperationException($"There is more than one {typeof(T).Name} instance in the scene");
                }
                if(managers.Length > 0)
                {
                    m_Instance = managers[0];
                }
                if(m_Instance == null)
                {
                    object[] customAttributes= typeof(T).GetCustomAttributes(typeof(SecureSingletonAttribute), false);
                    if (customAttributes.Length > 0 && customAttributes[0] is SecureSingletonAttribute attribute)
                    {
                        GameObject gO = new GameObject(typeof(T).Name);
                        m_Instance = gO.AddComponent<T>();
                    }
                    else
                    {
                        object[] singletonConfigs = typeof(T).GetCustomAttributes(typeof(SingletonConfiguration), false);
                        if (singletonConfigs.Length > 0 && singletonConfigs[0] is SingletonConfiguration singletonConfig)
                        {
                            string path = singletonConfig.ResourcesPath;
                            GameObject prefab = Resources.Load<GameObject>(path);
                            if (prefab == null)
                            {
                                throw new NullReferenceException($"There is no {typeof(T).Name} prefab in the resources folder");
                            }
                            GameObject obj = Instantiate(prefab);
                            m_Instance = obj.GetComponent<T>();
                            if (m_Instance == null)
                            {
                                throw new NullReferenceException($"There is no {typeof(T).Name} component attached to the singleton prefab");
                            }
                        }
                    }
                }
                DontDestroyOnLoad(m_Instance.gameObject);
            }
            return m_Instance;
        }
    }

    protected virtual void Awake()
    {
        if(m_Instance = null)
        {
            m_Instance = (T)this; //GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
        else if(m_Instance != this)
        {
            throw new InvalidOperationException($"There is more than one {typeof(T).Name} instance in the scene");
        }
    }
}