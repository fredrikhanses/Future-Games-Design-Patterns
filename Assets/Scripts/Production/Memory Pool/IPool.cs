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
        private bool disposed;
        private readonly uint expandBy;
        private readonly GameObject prefab;
        private readonly Transform parent;
        private readonly Stack<GameObject> objects = new Stack<GameObject>();
        private readonly List<GameObject> created = new List<GameObject>();

        public GameObjectPool(uint initSize, GameObject prefab, uint expandBy = 1, Transform parent = null)
        {
            this.expandBy = (uint)Mathf.Max(1f, expandBy);
            this.prefab = prefab;
            this.parent = parent;
            prefab.SetActive(false);
            Expand((uint)Mathf.Max(1f, initSize));
        }

        private void Expand(uint amount)
        {
            for (uint i = 0; i < amount; i++)
            {
                GameObject instance = Object.Instantiate(prefab, parent);
                EmitOnDisable emitOnDisable = instance.AddComponent<EmitOnDisable>();
                emitOnDisable.OnDisableGameObject += UnRent;
                //instance.tag.Replace(" ", "DontDestroy");
                objects.Push(instance);
                created.Add(instance);
            }
        }

        public void Clear()
        {
            foreach (GameObject gameObject in created)
            {
                if(gameObject != null)
                {
                    gameObject.GetComponent<EmitOnDisable>().OnDisableGameObject -= UnRent;
                    Object.Destroy(gameObject);
                }
            }
            objects.Clear();
            created.Clear();
        }

        private void UnRent(GameObject gameObject)
        {
            if(disposed == false)
            {
                objects.Push(gameObject);
            }
        }

        public GameObject Rent(bool returnActive)
        {
            if (disposed) return null;
            if (objects.Count == 0)
            {   
                Expand(expandBy);
                
            }
            GameObject instance = objects.Pop();
            instance.SetActive(returnActive);
            return instance;
        }

        public void Dispose()
        {
            disposed = true;
            Clear();
        }
    }
}