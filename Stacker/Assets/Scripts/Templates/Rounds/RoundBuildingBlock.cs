using System;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Templates.Rounds
{

    [Serializable]
    class RoundBuildingBlock
    {

        #region Editor

        [SerializeField] private GameObject prefab;
        [SerializeField] private int        quantity  = 1;
        [SerializeField] private bool       canRotate = true;

        #endregion

        #region Getters

        public GameObject Prefab
        {
            get
            {
                return prefab;
            }
        }

        public int Quantity
        {
            get
            {
                return quantity;
            }
        }

        public bool CanRotate
        {
            get
            {
                return canRotate;
            }
        }

        #endregion

    }

}
