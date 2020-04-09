using UnityEngine;

namespace Tools
{
    [CreateAssetMenu(menuName = "ScriptableObject/Pool/GameObject")]
    public class GameObjectScriptablePool : ScriptableObject, IPool<GameObject>
    {
        [SerializeField] private GameObject m_Prefab;
        [SerializeField] private uint m_InitSize;
        [SerializeField] private uint m_ExpandBy;
        [SerializeField] private bool m_HasParent;
        [SerializeField] private string m_ParentName;

        private const string k_ScriptablePool = "ScriptablePool";
        private GameObjectPool m_InternalPool;

        public GameObject Prefab { get => m_Prefab; }

        public GameObject Rent(bool returnActive)
        {
            if (m_InternalPool == null)
            {
                Transform parent = null;
                if (m_HasParent)
                {
                    parent = new GameObject(m_ParentName).transform;
                    parent.tag = k_ScriptablePool;
                }
                m_InternalPool = new GameObjectPool(m_InitSize, Prefab, m_ExpandBy, parent);
            }
            return m_InternalPool.Rent(returnActive);
        }

        public void OnDestroy()
        {
            m_InternalPool.Dispose();
        }
    }
}
