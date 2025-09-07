using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Core.GameStates;
using _Project.Scripts.Architecture.Core.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Architecture.Grid.Core
{
    public class GridSystem : GameControllable, IGridContext, IGridSystem
    {
        public Vector2Int GridSize => new(_width, _height);
        public Vector2 GridOffset => _gridOffset;
        public float CellSize => _cellSize;
        public Transform GridTransform => transform;
        public IGridCell[,] Cells => _cells;
        public GridCell CellPrefab => _cellPrefab;

        [Header("Grid Configuration")]
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private float _cellSize;
        [SerializeField] private Vector2 _gridOffset;
        [SerializeField] private GridCell _cellPrefab;
        
        private IGridGenerator _gridGenerator;
        private IPlacementSystem _placementSystem;
        private IHighlightSystem _highlightSystem;
        private Dictionary<Type, IGridComponent> _components;
        
        private IGridCell[,] _cells;
        private bool _isGridGenerated;
        
        private void Awake()
        {
            _components = new Dictionary<Type, IGridComponent>();

            _gridGenerator = GetComponent<IGridGenerator>();
            _placementSystem = GetComponent<IPlacementSystem>();
            _highlightSystem = GetComponent<IHighlightSystem>();
            
            InitializeComponents();
        }

        private void Start()
        {
            _gridGenerator?.GenerateGrid();
        }

        private void InitializeComponents()
        {
            if (_gridGenerator != null)
            {
                _gridGenerator.Initialize(this);
                _components.Add(typeof(IGridGenerator),_gridGenerator);
                
                _gridGenerator.OnGridGenerated += OnGridGenerated;
            }

            if (_placementSystem != null)
            {
                _placementSystem.Initialize(this);
                _components.Add(typeof(IPlacementSystem),_placementSystem);
            }
            
            if (_highlightSystem != null)
            {
                _highlightSystem.Initialize(this);
                _components.Add(typeof(IHighlightSystem), _highlightSystem);
            }
        }
        
        protected override void OnGameStateChanged(object sender, GameState newState)
        {
        }

        public void HighlightSuitableCells(CardUI card)
        {
            if (!_isGridGenerated) return;
            
            _highlightSystem.HighlightSuitableCells(card);
        }
        
        public void UnhighlightedCells()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var cell = _cells[x, y];
                    _highlightSystem.ClearHighlight(cell);
                }
            }
        }

        public bool TryPlaceCardOnGrid(CardUI card, Vector2 worldPosition)
        {
            if (!_isGridGenerated) return false;
            
            return _placementSystem.TryPlaceCard(card.CardData, worldPosition);
        }

        public bool TryGetGridComponent<TComponent>(out TComponent component) where TComponent : IGridComponent
        {
            if(_components.TryGetValue(typeof(TComponent), out var foundComponent) 
               && foundComponent is TComponent typedComponent)
            {
                component = typedComponent;
                return true;
            }
            component = default;
            return false;
        }
       
        private void OnGridGenerated(object sender, IGridCell[,] e)
        {
            _cells = e;
            _isGridGenerated = true;
            _gridGenerator.OnGridGenerated -= OnGridGenerated;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            _components.Values.ToList().ForEach(c => c.IsEnabled = false);
            _components.Clear();
            _isGridGenerated = false;
        }
        
        
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var gridWorldOrigin = transform.position + new Vector3(_gridOffset.x, _gridOffset.y, 0);
            if (Application.isPlaying)
            {
                Gizmos.color = Color.cyan;
                
                for (int x = 0; x < _width; x++)
                {
                    for (int y = 0; y < _height; y++)
                    {
                       var position = _gridGenerator.GetWorldPosition(this, x, y);
                        Gizmos.DrawWireCube(position, new Vector3(_cellSize, _cellSize, 0.1f));
                    }
                }
                
                // Візуалізація початкової точки
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(gridWorldOrigin, 0.2f);
            }
        }
#endif
        
    }
}