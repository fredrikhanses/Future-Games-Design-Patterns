using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tools
{
    public class ComponentPool<T> : IDisposable, IPool<T> where T : Component
    {
        private bool disposed;
        private readonly uint expandBy;
        private readonly Stack<T> objects;
        private readonly List<T> created;
        private readonly T prefab;
        private readonly Transform parent;

        public ComponentPool(uint initSize, T prefab, uint expandBy, Transform parent = null)
        {
            this.expandBy = expandBy;
            this.prefab = prefab;
            this.parent = parent;
            objects = new Stack<T>();
            created = new List<T>();
            Expand((uint)Mathf.Max(1f, initSize));
        }

        private void Expand(uint expandBy)
        {
            for (uint i = 0; i < expandBy; i++)
            {
                T instance = Object.Instantiate<T>(prefab, parent);
                instance.gameObject.AddComponent<EmitOnDisable>().OnDisableGameObject += UnRent;
                objects.Push(instance);
                created.Add(instance);
            }
        }

        public T Rent(bool returnActive)
        {
            if(objects.Count == 0)
            {
                Expand(expandBy);
            }

            T instance = objects.Pop();
            return instance;
        }
        
        private void UnRent(GameObject gameObject)
        {
            if(disposed == false)
            {
                objects.Push(gameObject.GetComponent<T>());
            }
        }

        public void Clean()
        {
            foreach (T component in created)
            {
                if (component != null)
                {
                    component.GetComponent<EmitOnDisable>().OnDisableGameObject -= UnRent;
                    Object.Destroy(component.gameObject);
                }
            }
            created.Clear();
            objects.Clear();
        }
        public void Dispose()
        {
            disposed = true;
            Clean();
        }
    }
}