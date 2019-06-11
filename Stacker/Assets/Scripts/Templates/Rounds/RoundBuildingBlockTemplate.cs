using System;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Templates.Rounds
{

    [Serializable]
    class RoundBuildingBlockTemplate
    {

        #region Editor

        [SerializeField] private BuildingBlockTemplate buildingBlockTemplate;
        [SerializeField] private int                   quantity  = 1;
        [SerializeField] private bool                  canRotate = true;

        #endregion

        #region Getters

        public GameObject Prefab
        {
            get
            {
                return buildingBlockTemplate.Prefab;
            }
        }

        public Mesh Mesh
        {
            get
            {
                return buildingBlockTemplate.Mesh;
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
