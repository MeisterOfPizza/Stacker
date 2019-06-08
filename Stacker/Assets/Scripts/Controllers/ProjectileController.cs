using Stacker.Components;
using Stacker.Extensions.Components;
using Stacker.Templates.Rounds;
using System.Collections;
using UnityEngine;

namespace Stacker.Controllers
{

    class ProjectileController : Controller<ProjectileController>
    {

        #region Editor

        [SerializeField] private GameObject projectilePrefab;

        [Space]
        [SerializeField] private float projectileSpawnRadiusPadding = 1f;

        #endregion

        #region Private variables

        private GameObjectPool<Projectile> projectilePool;

        #endregion

        public override void OnAwake()
        {
            projectilePool = new GameObjectPool<Projectile>(null, projectilePrefab, RoundChallengeTemplate.ROUND_CHALLENGE_FORTRESS_MAX_PROJECTILES);
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
            Projectile currentProjectile;

            for (int i = 0; i < projectilePool.UnavailableGameObjects.Count; i++)
            {
                currentProjectile = projectilePool.UnavailableGameObjects[i];

                yield return StartCoroutine(currentProjectile.FireProjectile());
            }

            projectilePool.DespawnAll();
        }

        #region Helper methods

        private Vector3 GetProjectilePosition()
        {
            float deg = Random.Range(0f, 360f);
            float radius = RoundController.Singleton.CurrentRound.BuildRadius + projectileSpawnRadiusPadding;

            return new Vector3(Mathf.Cos(deg) * radius, 0, Mathf.Sin(deg) * radius);
        }

        #endregion

    }

}
