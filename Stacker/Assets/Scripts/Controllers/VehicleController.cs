using Stacker.Components;
using Stacker.Extensions.Components;
using Stacker.Extensions.Utils;
using Stacker.Rounds;
using System;
using System.Collections;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Controllers
{

    class VehicleController : Controller<VehicleController>
    {

        #region Editor

        [SerializeField] private Transform    vehicleAnchor;
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
            vehiclePool = new GameObjectPool<Vehicle>(vehicleAnchor, vehiclePrefabs, 10);
        }

        public void SetupVehicles(TunnelChallenge challenge)
        {
            if (challenge != null)
            {
                Tuple<Vector3, Quaternion>[] vehiclePosRot = GetVehiclePositions();

                vehiclePool.Anchor.rotation = Quaternion.identity; // Reset rotation.

                for (int i = 0; i < RoundController.Singleton.CurrentRound.ProminentTunnelChallenge.Vehicles; i++)
                {
                    vehiclePool.Spawn(vehiclePosRot[i].Item1, vehiclePosRot[i].Item2).SetAsWarning();
                }

                int randomRot = 90 * UnityEngine.Random.Range(0, 4);
                vehiclePool.Anchor.rotation = Quaternion.Euler(0, randomRot, 0); // Apply new random rotation.
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

            vehiclePool.DespawnAll();
        }

        #region Helper methods

        private Tuple<Vector3, Quaternion>[] GetVehiclePositions()
        {
            float size = RoundController.Singleton.CurrentRound.BuildRadius + vehicleSpawnRadiusPadding;

            // We want to construct a vehicle pattern by returning Vector3s and Quaternions.
            
            // The grid will look like this:
            // ----- Z+ ------                                         
            // |             |    Cross   Tilted Cross  Highway  Oneway
            // |             |      v        v   v       v         v   
            // -X     mrrrrr +X  v     v                               
            // |      r      |      v        v   v          v          
            // |      r      |                                         
            // ----- Z- ------                                         
            // With m being the middle, r the radius and v the vehicle spawn.
            // Each pattern can be rotated 360 deg with 90 deg intervals, however, this will happen AFTER the vehicles have been spawned.

            switch (RoundController.Singleton.CurrentRound.ProminentTunnelChallenge.VehiclePattern)
            {
                case Templates.Rounds.TunnelChallengeVehiclePattern.Cross:
                    return new Tuple<Vector3, Quaternion>[]
                    {
                        new Tuple<Vector3, Quaternion>(new Vector3(-size, 0), Quaternion.LookRotation(Vector3.right)),  // Left of m
                        new Tuple<Vector3, Quaternion>(new Vector3(size, 0), Quaternion.LookRotation(Vector3.left)),    // Right of m
                        new Tuple<Vector3, Quaternion>(new Vector3(0, 0, size), Quaternion.LookRotation(Vector3.down)), // Above m
                        new Tuple<Vector3, Quaternion>(new Vector3(0, 0, -size), Quaternion.LookRotation(Vector3.up))   // Below m
                    };
                case Templates.Rounds.TunnelChallengeVehiclePattern.TiltedCross:
                    return new Tuple<Vector3, Quaternion>[]
                    {
                        new Tuple<Vector3, Quaternion>(new Vector3(-size, 0, size), Quaternion.LookRotation(-new Vector3(-size, 0, size).normalized)),  // Top left of m
                        new Tuple<Vector3, Quaternion>(new Vector3(size, 0, size), Quaternion.LookRotation(-new Vector3(size, 0, size).normalized)),    // Top right of m
                        new Tuple<Vector3, Quaternion>(new Vector3(size, 0, -size), Quaternion.LookRotation(-new Vector3(size, 0, -size).normalized)),  // Bottom right of m
                        new Tuple<Vector3, Quaternion>(new Vector3(-size, 0, -size), Quaternion.LookRotation(-new Vector3(-size, 0, -size).normalized)) // Bottom left of m
                    };
                case Templates.Rounds.TunnelChallengeVehiclePattern.Highway:

                    Tuple<Vector3, Quaternion> topLeft     = new Tuple<Vector3, Quaternion>(new Vector3(-size, 0, size), Quaternion.LookRotation(-new Vector3(-size, 0, size).normalized));
                    Tuple<Vector3, Quaternion> bottomRight = new Tuple<Vector3, Quaternion>(new Vector3(size, 0, -size), Quaternion.LookRotation(-new Vector3(size, 0, -size).normalized));

                    return new Tuple<Vector3, Quaternion>[]
                    {
                        topLeft,     // Top left of m
                        topLeft,     // Top left of m
                        bottomRight, // Bottom right of m
                        bottomRight  // Bottom right of m
                    };
                case Templates.Rounds.TunnelChallengeVehiclePattern.Oneway:

                    Tuple<Vector3, Quaternion> above = new Tuple<Vector3, Quaternion>(new Vector3(0, 0, size), Quaternion.LookRotation(Vector3.down));

                    return new Tuple<Vector3, Quaternion>[]
                    {
                        above,
                        above,
                        above,
                        above
                    };
                default:
                    return null;
            }
        }

        #endregion

    }

}
