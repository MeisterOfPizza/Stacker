using Stacker.Controllers;
using Stacker.Templates.RoundSurprises;
using System.Collections;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.RoundSurprises
{

    abstract class RoundSurprise : MonoBehaviour
    {

        #region Editor

        [Header("Base")]
        [SerializeField] private RoundSurpriseTemplate template;

        #endregion

        #region Getter

        public RoundSurpriseTemplate Template
        {
            get
            {
                return template;
            }
        }

        #endregion

        #region Abstract methods

        /// <summary>
        /// Spawn is called before <see cref="StartRoundSurprise"/>.
        /// </summary>
        public abstract void SpawnRoundSurprise();

        /// <summary>
        /// Start is called whenever the round surprise is starting.
        /// The <see cref="RoundSurpriseController"/> will wait for this coroutine to finish before moving on.
        /// </summary>
        public abstract IEnumerator StartRoundSurprise();

        /// <summary>
        /// Remove is called after the build phase is finished (<see cref="RoundSurpriseType.BeforeBuildPhase"/>) or after <see cref="StartRoundSurprise"/> is done.
        /// </summary>
        public abstract void RemoveRoundSurprise();

        #endregion

    }

}
