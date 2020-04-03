using UnityEngine;

namespace Tools
{
    [CreateAssetMenu(menuName = "ScriptableObject/Pool/GameObject")]
    public class GameObjectScriptablePool : ScriptableObject, IPool<GameObject>
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private uint initSize;
        [SerializeField] private uint expandBy;
        [SerializeField] private bool hasParent;
        [SerializeField] private string parentName;

        private GameObjectPool internalPool;

        public GameObject Rent(bool returnActive)
        {
            if (internalPool == null)
            {
                Transform parent = null;
                if (hasParent)
                {
                    parent = new GameObject(parentName).transform;
                }
                internalPool = new GameObjectPool(initSize, prefab, expandBy, parent);
            }
            return internalPool.Rent(returnActive);
        }

        public void OnDestroy()
        {
            internalPool.Dispose();
        }
    }
}
