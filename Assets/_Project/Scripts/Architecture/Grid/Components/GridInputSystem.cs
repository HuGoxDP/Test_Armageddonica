using System;
using _Project.Scripts.Architecture.Core.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Architecture.Grid.Components
{
    public class GridInputSystem : MonoBehaviour, IGridInputSystem
    {
        public event Action<IGridCell> OnCellHoverEnter;
        public event Action<IGridCell> OnCellHoverExit;
        public bool IsEnabled { get; set; }
        
        [Header("Input Settings")]
        [SerializeField] private LayerMask _cellLayerMask = 1 << 0;
        
        [Header("Performance Settings")]
        [SerializeField] private float _inputCheckInterval = 0.1f;
        
        private Camera _mainCamera;
        private IGridCell _lastHoveredCell;
        private LayerMask _gridLayerMask;
        
        private float _lastInputCheckTime;

        private void Start()
        {
            _mainCamera = Camera.main;
            if (_mainCamera == null)
            {
                Debug.LogError("Main camera not found!");
                enabled = false;
                return;
            }
            _gridLayerMask = _cellLayerMask;
        }

        private void Update()
        {
            if (!IsEnabled) return;

            if (Time.time - _lastInputCheckTime >= _inputCheckInterval)
            {
                HandleGridInput();
                _lastInputCheckTime = Time.time;
            }
        }

        public void Initialize(IGridContext gridContext)
        {
            IsEnabled = true;
        }
        
        private void HandleGridInput()
        {
            if (!IsEnabled) return;
            try
            {
                Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                Collider2D hit = Physics2D.OverlapPoint(mousePos, _gridLayerMask);
                if (hit != null)
                {
                    var cell = hit.GetComponent<IGridCell>();
                    if (cell != null)
                    {
                        if (_lastHoveredCell != cell)
                        {
                            if (_lastHoveredCell != null)
                                OnCellHoverExit?.Invoke(_lastHoveredCell);

                            _lastHoveredCell = cell;
                            OnCellHoverEnter?.Invoke(cell);
                        }

                        return;
                    }
                }

                if (_lastHoveredCell != null)
                {
                    OnCellHoverExit?.Invoke(_lastHoveredCell);
                    _lastHoveredCell = null;
                }

            }
            catch (Exception e)
            {
                Debug.LogError($"Error in GridInputSystem: {e.Message}");
            }
        }
        
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !IsEnabled) return;
            
            Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
    
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(mousePos, 0.1f);
    
            if (_lastHoveredCell != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(_lastHoveredCell.GameObject.transform.position, Vector3.one * 0.5f);
            }
        }
    }
}