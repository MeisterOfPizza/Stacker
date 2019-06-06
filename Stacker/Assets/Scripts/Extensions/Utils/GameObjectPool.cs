using System.Collections.Generic;
using UnityEngine;

namespace Stacker.Extensions.Components
{

    class GameObjectPool
    {

        #region Private variables

        private Transform anchor;

        private List<GameObject> availableGameObjects;
        private List<GameObject> unavailableGameObjects;

        #endregion

        #region Public properties

        public Transform Anchor
        {
            get
            {
                return anchor;
            }
        }

        #endregion

        public GameObjectPool(Transform anchor, GameObject[] prefabs, int maxPrefabInstances)
        {
            this.anchor = anchor;

            int poolSize = prefabs.Length * maxPrefabInstances;

            availableGameObjects   = new List<GameObject>(poolSize + 5); // Give a little extra buffer space so that it doesn't have to resize instantly.
            unavailableGameObjects = new List<GameObject>(poolSize + 5); // -||-

            CreatePrefabPool(prefabs, maxPrefabInstances);
        }

        private void CreatePrefabPool(GameObject[] prefabs, int maxPrefabInstances)
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                for (int j = 0; j < maxPrefabInstances; j++)
                {
                    availableGameObjects.Add(GameObject.Instantiate(prefabs[i], anchor));

                    // Deactivate the newly created GameObject:
                    availableGameObjects[unavailableGameObjects.Count - 1].SetActive(false);
                }
            }
        }

        public GameObject Spawn(Vector3 position, Quaternion rotation)
        {
            if (availableGameObjects.Count > 0)
            {
                int index = Random.Range(0, availableGameObjects.Count);

                GameObject taken = availableGameObjects[index];
                availableGameObjects.RemoveAt(index);

                unavailableGameObjects.Add(taken);

                taken.transform.SetPositionAndRotation(position, rotation);
                taken.SetActive(true);

                return taken;
            }

            return null;
        }

        public void DespawnAll()
        {
            for (int i = 0; i < unavailableGameObjects.Count; i++)
            {
                availableGameObjects.Add(unavailableGameObjects[i]);
            }

            unavailableGameObjects.Clear();
        }

    }

}
