using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner
{
    public abstract class GenericPooling : MonoBehaviour
    {
        [SerializeField]
        private ItemBase prefab;
        [SerializeField]
        private int poolSize = 1;
        [SerializeField]
        private bool poolCanGrow = false;

        private List<ItemBase> pool = new List<ItemBase>();

        void Awake()
        {
            if (prefab == null)
            {
                Debug.LogError("Has not been defined a prefab!");
            }

            GeneratePool();
        }

        protected ItemBase GetObjectFromPool(bool active = true)
        {
            for (int i = 0; i < pool.Count; i++)
            {
                var obj = pool[i];

                if (!obj.gameObject.activeInHierarchy)
                {
                    obj.gameObject.SetActive(active);

                    return obj;
                }
            }

            if (poolCanGrow)
            {
                var obj = CreateNewObject();

                obj.gameObject.SetActive(active);

                return obj;
            }

            return null;
        }

        protected void DeactivateAll()
        {
            for (int i = 0; i < pool.Count; i++)
            {
                pool[i].gameObject.SetActive(false);

                pool[i].transform.position = Vector3.zero;
            }
        }

        private void GeneratePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                CreateNewObject();
            }
        }

        private ItemBase CreateNewObject()
        {
            var newObject = Instantiate(prefab, transform.position, transform.rotation) as ItemBase;
            newObject.gameObject.SetActive(false);
            newObject.transform.SetParent(transform);

            pool.Add(newObject);

            return newObject;
        }
    }
}