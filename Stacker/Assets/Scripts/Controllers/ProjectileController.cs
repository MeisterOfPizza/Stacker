using Stacker.Components;
using Stacker.Extensions.Components;
using Stacker.Templates.Rounds;
using System.Collections;
using System.Collections.Generic;
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

        private GameObjectPool projectilePool;

        private List<Projectile> projectiles;

        #endregion

        public override void OnAwake()
        {
            projectilePool = new GameObjectPool(null, new GameObject[] { projectilePrefab }, RoundChallengeTemplate.ROUND_CHALLENGE_FORTRESS_MAX_PROJECTILES);

            projectiles = new List<Projectile>(RoundChallengeTemplate.ROUND_CHALLENGE_FORTRESS_MAX_PROJECTILES + 5); // Add extra buffer size.
        }

        public void SetupProjectiles()
        {
            projectilePool.DespawnAll();
            projectiles.Clear();

            for (int i = 0; i < RoundController.Singleton.CurrentRound.MaxProjectiles; i++)
            {
                projectiles.Add(projectilePool.Spawn(GetProjectilePosition(), Quaternion.identity).GetComponent<Projectile>());
            }
        }

        public IEnumerator FireProjectiles()
        {
            Projectile currentProjectile;

            while (projectiles.Count > 0)
            {
                currentProjectile = projectiles[0];
                projectiles.RemoveAt(0);

                yield return StartCoroutine(currentProjectile.FireProjectile());
            }
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
