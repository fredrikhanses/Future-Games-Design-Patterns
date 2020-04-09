using Tools;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //[SerializeField] private Bullet bulletComponentPrefab;
    //[SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObjectScriptablePool m_BulletScriptablePool;

    //private GameObjectPool bulletPool;
    //private ComponentPool<Bullet> bulletComponentPool;

    //private GameObject bulletParent;
    //private GameObject bulletComponentParent;

    //private void Awake()
    //{
    //    bulletComponentParent = new GameObject("Bullet Component Parent")
    //    {
    //        tag = "DontDestroy"
    //    };
    //    bulletParent = new GameObject("Bullet Parent")
    //    {
    //        tag = "DontDestroy"
    //    };
    //    bulletPool = new GameObjectPool(10, bulletPrefab, 5, bulletParent.transform);
    //    bulletComponentPool = new ComponentPool<Bullet>(1, bulletComponentPrefab, 1, bulletComponentParent.transform);
    //}

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
            /// <summary>
            ///    GameObjectPool
            /// </summary>
            //GameObject bullet = bulletPool.Rent(false);
            //Bullet bulletComponent = bullet.GetComponent<Bullet>();
            //bullet.transform.position = transform.position;
            //bulletComponent.Reset();
            //bullet.GetComponent<Renderer>().material.color = Random.ColorHSV(0.8f, 1f);
            //bullet.SetActive(true);
            //bulletComponent.Push();

            /// <summary>
            ///    ComponentPool
            /// </summary>
            //Bullet bulletComponent = bulletComponentPool.Rent(false);
            //bulletComponent.transform.position = transform.position;
            //bulletComponent.Reset();
            //bulletComponent.GetComponent<Renderer>().material.color = Random.ColorHSV(0.8f, 1f);
            //bulletComponent.gameObject.SetActive(true);
            //bulletComponent.Push();

            /// <summary>
            ///    ScriptableObjectPool
            /// </summary>
    //        GameObject bullet = scriptablePool.Rent(true);
    //        Bullet bulletComponent = bullet.GetComponent<Bullet>();
    //        bulletComponent.Reset();
    //        bulletComponent.transform.position = transform.position;
    //        bulletComponent.Push(Random.onUnitSphere);
    //    }
    //}

    public void Shoot(Vector3 direction)
    {
        GameObject bullet = m_BulletScriptablePool.Rent(true);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.Reset();
        bulletComponent.transform.position = transform.position;
        bulletComponent.Push(direction);
    }

    //private void OnDestroy()
    //{
    //    bulletPool.Dispose();
    //    bulletComponentPool.Dispose();
    //}
}
