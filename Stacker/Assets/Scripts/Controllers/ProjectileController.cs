using Stacker.Extensions.Components;
using Stacker.Extensions.Utils;
using Stacker.RoundAction;
using Stacker.Templates.Rounds;
using System.Collections;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Controllers
{

    class ProjectileController : Controller<ProjectileController>
    {

        #region Editor

        [SerializeField] private Transform  projectileContainer;
        [SerializeField] private GameObject projectilePrefab;

        [Header("Spawning settings")]
        [SerializeField] private float projectileSpawnRadiusPadding = 1f;
        [SerializeField] private float projectileSpawnHeight = 1f;

        [Header("Misc")]
        [SerializeField] private LayerMask structureLayerMask;

        #endregion

        #region Private variables

        private GameObjectPool<Projectile> projectilePool;

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
            projectilePool = new GameObjectPool<Projectile>(projectileContainer, projectilePrefab, RoundChallengeTemplate.ROUND_CHALLENGE_FORTRESS_MAX_PROJECTILES, Vector3.one);
        }

        public void ClearProjectiles()
        {
            projectilePool.DespawnAll();
        }

        public void SetupProjectiles()
        {
            for (int i = 0; i < RoundController.Singleton.CurrentRound.MaxProjectiles; i++)
            {
                projectilePool.Spawn(GetProjectilePosition(), Quaternion.identity).SetAsWarning();
            }
        }

        public IEnumerator FireProjectiles()
        {
            ChainEventQueue<Projectile> chainEventQueue = new ChainEventQueue<Projectile>(projectilePool.UnavailableGameObjects);
            chainEventQueue.StartChain();

            while (!chainEventQueue.IsChainDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        #region Helper methods

        private Vector3 GetProjectilePosition()
        {
            float deg = Random.Range(0f, 360f);
            float radius = RoundController.Singleton.CurrentRound.BuildRadius + projectileSpawnRadiusPadding;

            return new Vector3(Mathf.Cos(deg * Mathf.Deg2Rad) * radius, projectileSpawnHeight, Mathf.Sin(deg * Mathf.Deg2Rad) * radius);
        }

        #endregion

    }

}
