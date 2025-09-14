using System;
using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Tooltip;
using UnityEngine;

namespace _Project.Scripts.Architecture.Grid.Components
{
    public class GridTooltipManager : MonoBehaviour, IGridTooltipManager
    {
        private IGridInputSystem _gridInputSystem;
        private StatsTooltipUI _statsTooltipUI; 
        public bool IsEnabled { get; set; }
        private bool _isEnableTooltips = true;

        private void OnDestroy()
        {
            if (_gridInputSystem != null)
            {
                _gridInputSystem.OnCellHoverEnter -= OnCellHoverEnter;
                _gridInputSystem.OnCellHoverExit -= OnCellHoverExit;
            }
        }

        private void Start()
        {
            _statsTooltipUI = ServiceLocator.Get<StatsTooltipUI>();
            
            if (_statsTooltipUI == null)
                Debug.LogError("StatsTooltipUI reference is not assigned in GridTooltipManager.");
        }

        public void RegisterGridInputSystem(IGridInputSystem gridInputSystem)
        {
            _gridInputSystem = gridInputSystem;
            if (_gridInputSystem != null)
            {
                _gridInputSystem.OnCellHoverEnter += OnCellHoverEnter;
                _gridInputSystem.OnCellHoverExit += OnCellHoverExit;
            }
            else
            {
                Debug.LogError("GridInputSystem reference is not assigned in GridTooltipManager.");
            }
        }
        
        public void Initialize(IGridContext gridContext)
        {
            IsEnabled = true;
        }  
        
        public void EnableTooltips(bool isEnable)
        {
            if(!IsEnabled) return;
            if(_statsTooltipUI == null) return;
            if (isEnable)
            {
                _isEnableTooltips = true;
            }
            else
            {
                _isEnableTooltips = false;
                _statsTooltipUI.HideTooltip();
            }
        }
        
        private void OnCellHoverEnter(IGridCell cell)
        {   
            if(!IsEnabled) return;
            if (cell == null) return;
            
            if(_statsTooltipUI == null) return;
            if (cell.IsOccupied && _isEnableTooltips)
            {
                _statsTooltipUI.ShowTooltip();
                _statsTooltipUI.SetTooltip(cell.OccupiedEntity);
            }
        }
        
        private void OnCellHoverExit(IGridCell cell)
        {
            if(!IsEnabled) return;
            if(_statsTooltipUI == null) return;
            _statsTooltipUI.HideTooltip();
        }
      
    }
}