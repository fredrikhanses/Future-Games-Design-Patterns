using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tools
{
    public interface IPool<T>
    {
        T Rent(bool returnActive);
    }

    public class GameObjectPool : IPool<GameObject>, IDisposable
    {
        private bool m_Disposed;
        private readonly uint m_ExpandBy;
        private readonly GameObject m_Prefab;
        private readonly Transform m_Parent;
        private readonly Stack<GameObject> m_Objects = new Stack<GameObject>();
        private readonly List<GameObject> m_Created = new List<GameObject>();

        public GameObjectPool(uint initSize, GameObject prefab, uint expandBy = 1, Transform parent = null)
        {
            m_ExpandBy = (uint)Mathf.Max(1f, expandBy);
            m_Prefab = prefab;
            m_Parent = parent;
            prefab.SetActive(false);
            Expand((uint)Mathf.Max(1f, initSize));
        }

        private void Expand(uint amount)
        {
            for (uint i = 0; i < amount; i++)
            {
                GameObject instance = Object.Instantiate(m_Prefab, m_Parent);
                EmitOnDisable emitOnDisable = instance.AddComponent<EmitOnDisable>();
                emitOnDisable.OnDisableGameObject += UnRent;
                m_Objects.Push(instance);
                m_Created.Add(instance);
            }
        }

        public void Clear()
        {
            foreach (GameObject gameObject in m_Created)
            {
                if(gameObject != null)
                {
                    gameObject.GetComponent<EmitOnDisable>().OnDisableGameObject -= UnRent;
                    Object.Destroy(gameObject);
                }
            }
            m_Objects.Clear();
            m_Created.Clear();
        }

        private void UnRent(GameObject gameObject)
        {
            if(m_Disposed == false)
            {
                m_Objects.Push(gameObject);
            }
        }

        public GameObject Rent(bool returnActive)
        {
            if (m_Disposed) return null;
            if (m_Objects.Count == 0)
            {   
                Expand(m_ExpandBy);
                
            }
            GameObject instance = m_Objects.Pop();
            instance.SetActive(returnActive);
            return instance;
        }

        public void Dispose()
        {
            m_Disposed = true;
            Clear();
        }
    }
}