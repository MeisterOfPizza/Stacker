﻿using UnityEngine;

namespace Stacker.Controllers
{

    /// <summary>
    /// Base class for all controllers.
    /// Do not override Unity's default Awake, use <see cref="OnAwake"/> instead.
    /// </summary>
    abstract class Controller<T> : MonoBehaviour where T : Controller<T>
    {

        #region Static properties

        public static T Singleton { get; protected set; }

        #endregion

        #region MonoBehaviour methods

        private void Awake()
        {
            Singleton = (T)this;

            OnAwake();
        }

        public virtual void OnAwake()
        {
            // Do nothing here.
            // Not all classes that inherit from this will use the awake methods.
        }

        #endregion

    }

}
