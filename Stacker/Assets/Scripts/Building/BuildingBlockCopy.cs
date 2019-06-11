﻿using Stacker.Controllers;
using UnityEngine;

#pragma warning disable 0649

namespace Stacker.Building
{

    class BuildingBlockCopy : MonoBehaviour
    {

        #region Editor

        [Header("References")]
        [SerializeField] private MeshRenderer meshRenderer;

        [Header("Selection")]
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _selectedMaterial;

        #endregion

        #region Private variables

        private BuildingBlock buildingBlock;

        private BuildingBlockState state = BuildingBlockState.Inactive;

        #endregion

        #region Public properties

        public BuildingBlock BuildingBlock
        {
            get
            {
                return buildingBlock;
            }
        }

        public BuildingBlockState State
        {
            get
            {
                return state;
            }
        }

        public bool CanRotate
        {
            get
            {
                return buildingBlock.RoundBuildingBlockTemplate.CanRotate;
            }
        }

        #endregion

        #region Enums

        internal enum BuildingBlockState
        {
            Inactive,
            Active,
            Selected
        }

        #endregion

        public void Initialize(BuildingBlock buildingBlock)
        {
            this.buildingBlock = buildingBlock;
        }

        #region MonoBehaviour methods

        private void OnEnable()
        {
            state = BuildingBlockState.Active;
        }

        private void OnDisable()
        {
            state = BuildingBlockState.Inactive;
        }

        #endregion

        #region Selection
        
        public void Select()
        {
            if (state != BuildingBlockState.Selected)
            {
                state = BuildingBlockState.Selected;

                meshRenderer.material = _selectedMaterial;
            }
        }

        public void Deselect()
        {
            if (state == BuildingBlockState.Selected)
            {
                state = BuildingBlockState.Active;

                meshRenderer.material = _defaultMaterial;
            }
        }

        #endregion

        #region Positioning

        public void PlaceBuildingBlock(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
        }

        #endregion

        #region Events systems

        private void OnMouseDown()
        {
            BuildController.Singleton.SelectCopy(this);
        }

        #endregion

    }

}