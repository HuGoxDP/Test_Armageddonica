

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Core.GameStates;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Entities.Base;
using _Project.Scripts.Architecture.Enums;
using _Project.Scripts.Architecture.Tooltip;
using UnityEngine;

namespace _Project.Scripts.Architecture.Grid.Core
{
    public class GridSystem : GameControllable, IGridContext, IGridSystem
    {
        public event EventHandler<EntityPlacementEventArgs> OnEntityPlaced;
        public event EventHandler<EntityPlacementEventArgs> OnEntityRemoved;
        
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
        private IGridInputSystem _gridInputSystem;
        private Dictionary<Type, IGridComponent> _components;

        private IGridCell[,] _cells;
        private StatsTooltipUI _statsTooltipUI;
        private bool _isGridGenerated;
        private bool _isEnableTooltips = true;
        private void Awake()
        {
            ServiceLocator.Register(this);
            _components = new Dictionary<Type, IGridComponent>();

            _gridGenerator = GetComponent<IGridGenerator>();
            _placementSystem = GetComponent<IPlacementSystem>();
            _highlightSystem = GetComponent<IHighlightSystem>();
            _gridInputSystem = GetComponent<IGridInputSystem>();

            InitializeComponents();
        }

        private void Start()
        {
            _gridGenerator?.GenerateGrid();
            _statsTooltipUI = ServiceLocator.Get<StatsTooltipUI>();
            if (_statsTooltipUI == null) Debug.LogError("StatsTooltipUI reference is not assigned in GridSystem.");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _components.Values.ToList().ForEach(c => c.IsEnabled = false);
            _components.Clear();
            _isGridGenerated = false;
           
            if (_placementSystem != null)
            {
                _placementSystem.OnEntityPlaced -= OnEntityPlacementEvent;
                _placementSystem.OnEntityRemoved -= OnEntityPlacementEvent;
            }
        }

        public void HighlightSuitableCells(CardUI card)
        {
            if (!_isGridGenerated) return;

            _highlightSystem.HighlightSuitableCells(card);
        }

        public void UnhighlightedCells()
        {
           if (!_isGridGenerated) return;
            
           _highlightSystem.UnhighlightedCells();
        }

        public async Task<bool> TryPlaceCardOnGrid(CardUI card, Vector2 worldPosition)
        {
            if (!_isGridGenerated || _placementSystem == null) 
                return false;
            
            return await _placementSystem.TryPlaceCard(card.CardData, worldPosition);
        }

        public bool CanPlaceAt(BaseCardData cardData, IGridCell cell)
        {
            return _isGridGenerated && _placementSystem.CanPlaceAt(cardData, cell);
        }

        public bool TryGetCell(Vector3 position, out IGridCell cell)
        {
            cell = null;
            if (!_isGridGenerated) return false;

            cell = _gridGenerator.GetCellAt(position);
            return cell != null;
        }

        public List<Entity> GetEntitiesInRange(Vector2Int position, int range)
        {
            if (!_isGridGenerated) return null;
            if (position.x < 0 || position.x >= _width || position.y < 0 || position.y >= _height) return null;
            var resultList = new List<Entity>();
            
            for (int x = position.x - range; x <= position.x + range; x++)
            {
                for (int y = position.y - range; y <= position.y + range; y++)
                {
                    if (position.x == x && position.y == y) continue;
                    if (x < 0 || x >= _width || y < 0 || y >= _height) continue;
                    var cell = _cells[x, y];
                    if (cell == null) continue;

                    var entity = cell.OccupiedEntity;

                    if (entity == null) continue;

                    resultList.Add(entity);
                }
            }
            
            return resultList;
        }

        public List<Entity> GetEntities()
        {
            var entities = new List<Entity>();
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var cell = _cells[x, y];
                    if (cell == null) continue;

                    var entity = cell.OccupiedEntity;

                    if (entity == null) continue;
                    entities.Add(entity);
                }
            }

            if (entities.Count == 0) return null;
            return entities;
        }

        private void InitializeComponents()
        {
            if (_gridGenerator != null)
            {
                _gridGenerator.Initialize(this);
                _components.Add(typeof(IGridGenerator), _gridGenerator);

                _gridGenerator.OnGridGenerated += OnGridGenerated;
            }

            if (_placementSystem != null)
            {
                _placementSystem.Initialize(this);
                _components.Add(typeof(IPlacementSystem), _placementSystem);

                _placementSystem.OnEntityPlaced += OnEntityPlacementEvent;
                _placementSystem.OnEntityRemoved += OnEntityPlacementEvent;
            }

            if (_highlightSystem != null)
            {
                _highlightSystem.Initialize(this);
                _components.Add(typeof(IHighlightSystem), _highlightSystem);
            }

            if (_gridInputSystem != null)
            {
                _gridInputSystem.Initialize(this);
                _components.Add(typeof(IGridInputSystem), _gridInputSystem);
                
                _gridInputSystem.OnCellHoverEnter += OnCellHoverEnter;
                _gridInputSystem.OnCellHoverExit += OnCellHoverExit;
            }
        }

        private void OnCellHoverEnter(IGridCell cell)
        {   if (cell == null) return;
            
            //hoverLogic
            _highlightSystem.HighlightHoverCell(cell);
            
            //tooltip logic
            if(_statsTooltipUI == null) return;
            if (cell.IsOccupied && _isEnableTooltips)
            {
                _statsTooltipUI.ShowTooltip();
                _statsTooltipUI.SetTooltip(cell.OccupiedEntity);
            }
        }

        private void OnCellHoverExit(IGridCell cell)
        {
            //hoverLogic
            _highlightSystem.ClearHoverHighlight();
            
            //tooltip logic
            if(_statsTooltipUI == null) return;
            _statsTooltipUI.HideTooltip();
        }

        public void EnableTooltips(bool isEnable)
        {
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

        private void OnGridGenerated(object sender, IGridCell[,] e)
        {
            _cells = e;
            _isGridGenerated = true;
            _gridGenerator.OnGridGenerated -= OnGridGenerated;
        }
        
        private void OnEntityPlacementEvent(object sender, EntityPlacementEventArgs e)
        {
            if (e.IsPlaced)
            {
                OnEntityPlaced?.Invoke(this, e);
            }
            else
            {
                OnEntityRemoved?.Invoke(this, e);
            }
        }
        

        protected override void OnGameStateChanged(object sender, GameState newState)
        {
            EnableTooltips(newState == GameState.CardPlacementTurn);
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

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(gridWorldOrigin, 0.2f);
            }
        }
#endif
    }
}