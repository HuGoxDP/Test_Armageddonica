using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Core.GameStates;
using UnityEngine;

namespace _Project.Scripts.Architecture.Grid.Core
{
    public class GridSystem : GameControllable
    {
        [SerializeField] private int _width = 5;
        [SerializeField] private int _height = 5;
        [SerializeField] private int _cellSize = 1;
        [SerializeField] private GridCell _cellPrefab;

        private GridCell[,] _cells;
        private MatchController _matchController;


        private void Awake()
        {
            _cells = new GridCell[_width, _height];
        }

        private void Start()
        {
            GenerateGrid();
        }

        public override void OnGameStateChanged(object sender, GameState newState)
        {
           
        }
        
        /// <summary> Generates the grid based on the specified width, height, and cell size. </summary>
        private void GenerateGrid()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    float xPosition = x * _cellSize;
                    float yPosition = y * _cellSize;

                    GridCell newCell = Instantiate(
                        _cellPrefab,
                        new Vector3(xPosition, yPosition, 0),
                        Quaternion.identity,
                        transform
                    );
                    newCell.name = $"Cell_{x}_{y}";

                    _cells[x, y] = newCell;
                }
            }
        }

        /// <summary> Gets the world position of the center of a cell at the specified grid coordinates. </summary>
        public GridCell GetCellAtPosition(Vector3 worldPosition)
        {
            int x = Mathf.FloorToInt(worldPosition.x / _cellSize);
            int y = Mathf.FloorToInt(worldPosition.y / _cellSize);

            if (x < 0 || x >= _width || y < 0 || y >= _height)
            {
                return null;
            }

            return _cells[x, y];
        }

        /// <summary> Checks if an object can be placed at the specified world position. </summary>
        public bool CanPlaceAt(Vector3 position)
        {
            if (GetCellAtPosition(position) == null)
            {
                return false;
            }
            // TODO: Перевірити, чи можна розмістити об'єкт на цій позиції
            return true;
        }
        
        
        /// <summary> Highlights or unhighlights the cell at the specified world position. </summary>
        private void HighlightCellAtPosition(Vector3 worldPosition, bool highlight)
        {
            var cell = GetCellAtPosition(worldPosition);
            if (cell != null)
            {
                cell.Highlight(highlight);
            }
        }
        public void PlaceEntityAt(GridCell cell, BaseCardData cardUICardData)
        {
            if (cell == null || cell.IsOccupied)
            {
                Debug.LogWarning("Cannot place entity: Cell is null or already occupied.");
                return;
            }
            //TDOD : Use EntityFactory to create entity from card data
            cell.SetEntity(null);
        }
    }
}