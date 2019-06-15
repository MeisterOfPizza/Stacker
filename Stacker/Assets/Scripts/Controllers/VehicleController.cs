using Stacker.Components;
using Stacker.Extensions.Components;
using Stacker.Extensions.Utils;
using Stacker.Rounds;
using Stacker.Templates.Rounds;
using System.Collections;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Controllers
{

    class VehicleController : Controller<VehicleController>
    {

        #region Editor

        [SerializeField] private Transform    vehicleContainer;
        [SerializeField] private GameObject[] vehiclePrefabs;

        [Header("Spawning settings")]
        [SerializeField] private float vehicleSpawnRadiusPadding = 1f;

        [Header("Misc")]
        [SerializeField] private LayerMask structureLayerMask;

        #endregion

        #region Private variables

        private GameObjectPool<Vehicle> vehiclePool;

        #endregion

        #region Public properties

        public LayerMask StructureLayerMask
        {
            get
            {
                return structureLayerMask;
            }
        }

        #endregion

        public override void OnAwake()
        {
            //TEST: Change the number of max prefab instances:
            vehiclePool = new GameObjectPool<Vehicle>(vehicleContainer, vehiclePrefabs, RoundChallengeTemplate.ROUND_CHALLENGE_TUNNEL_MAX_VEHICLES, Vector3.one);
        }

        public void ClearVehicles()
        {
            vehiclePool.DespawnAll();
        }

        public void SetupVehicles()
        {
            for (int i = 0; i < RoundController.Singleton.CurrentRound.MaxVehicles; i++)
            {
                Vector3 spawnPos = GetVehiclePosition();
                Quaternion direction = Quaternion.LookRotation(-spawnPos); // Look at middle (same as Vector3.zero - spawnPos).

                vehiclePool.Spawn(spawnPos, direction).SetAsWarning();
            }
        }

        public IEnumerator LaunchVehicles()
        {
            ChainEventQueue<Vehicle> chainEventQueue = new ChainEventQueue<Vehicle>(vehiclePool.UnavailableGameObjects);
            chainEventQueue.StartChain();

            while (!chainEventQueue.IsChainDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        #region Helper methods

        private Vector3 GetVehiclePosition()
        {
            float deg = Random.Range(0f, 360f);
            float radius = RoundController.Singleton.CurrentRound.BuildRadius + vehicleSpawnRadiusPadding;

            return new Vector3(Mathf.Cos(deg * Mathf.Deg2Rad) * radius, 0, Mathf.Sin(deg * Mathf.Deg2Rad) * radius);
        }

        #endregion

    }

}
