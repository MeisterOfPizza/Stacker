using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stacker.Extensions.Components
{

    class GameObjectPool<T> where T : MonoBehaviour
    {

        #region Private variables

        private Transform anchor;

        private List<T> availableGameObjects;
        private List<T> unavailableGameObjects;

        #endregion

        #region Public properties

        public Transform Anchor
        {
            get
            {
                return anchor;
            }
        }

        public List<T> AvailableGameObjects
        {
            get
            {
                return availableGameObjects.ToList();
            }
        }

        public List<T> UnavailableGameObjects
        {
            get
            {
                return unavailableGameObjects.ToList();
            }
        }

        #endregion

        #region Constructors

        public GameObjectPool(Transform anchor, GameObject prefab, int maxPrefabInstances)
        {
            this.anchor = anchor;

            availableGameObjects   = new List<T>(maxPrefabInstances + 5); // Give a little extra buffer space so that it doesn't have to resize instantly.
            unavailableGameObjects = new List<T>(maxPrefabInstances + 5); // -||-

            CreatePrefabPool(new GameObject[] { prefab }, maxPrefabInstances);
        }

        public GameObjectPool(Transform anchor, GameObject[] prefabs, int maxPrefabInstances)
        {
            this.anchor = anchor;

            int poolSize = prefabs.Length * maxPrefabInstances;

            availableGameObjects   = new List<T>(poolSize + 5); // Give a little extra buffer space so that it doesn't have to resize instantly.
            unavailableGameObjects = new List<T>(poolSize + 5); // -||-

            CreatePrefabPool(prefabs, maxPrefabInstances);
        }

        #endregion

        #region Setup

        private void CreatePrefabPool(GameObject[] prefabs, int maxPrefabInstances)
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                for (int j = 0; j < maxPrefabInstances; j++)
                {
                    availableGameObjects.Add(GameObject.Instantiate(prefabs[i], anchor).GetComponent<T>());

                    // Deactivate the newly created GameObject:
                    availableGameObjects[availableGameObjects.Count - 1].gameObject.SetActive(false);
                }
            }
        }

        #endregion

        #region Pool methods

        public T Spawn()
        {
            if (availableGameObjects.Count > 0)
            {
                int index = Random.Range(0, availableGameObjects.Count);

                T taken = availableGameObjects[index];
                availableGameObjects.RemoveAt(index);

                unavailableGameObjects.Add(taken);

                taken.gameObject.SetActive(true);

                return taken;
            }

            return null;
        }

        public T Spawn(Vector3 position, Quaternion rotation)
        {
            if (availableGameObjects.Count > 0)
            {
                int index = Random.Range(0, availableGameObjects.Count);

                T taken = availableGameObjects[index];
                availableGameObjects.RemoveAt(index);

                unavailableGameObjects.Add(taken);

                taken.transform.SetPositionAndRotation(position, rotation);
                taken.gameObject.SetActive(true);

                return taken;
            }

            return null;
        }

        public bool Despawn(T item)
        {
            bool existed = unavailableGameObjects.Remove(item);

            if (existed)
            {
                availableGameObjects.Add(item);

                item.gameObject.SetActive(false);
            }

            return existed;
        }

        public void DespawnAll()
        {
            for (int i = 0; i < unavailableGameObjects.Count; i++)
            {
                availableGameObjects.Add(unavailableGameObjects[i]);

                unavailableGameObjects[i].gameObject.SetActive(false);
            }

            unavailableGameObjects.Clear();
        }

        public void DestroyAll()
        {
            foreach (T item in unavailableGameObjects)
            {
                GameObject.Destroy(item.gameObject);
            }

            foreach (T item in availableGameObjects)
            {
                GameObject.Destroy(item.gameObject);
            }
        }

        #endregion

    }

}
