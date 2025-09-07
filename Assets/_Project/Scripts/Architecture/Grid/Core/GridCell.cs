using _Project.Scripts.Architecture.Entities.Base;
using UnityEngine;

namespace _Project.Scripts.Architecture.Grid.Core
{
    public class GridCell : MonoBehaviour, IGridCell
    {
        public GameObject GameObject => gameObject;
        public IGridCell[] Neighbors {get; private set;}
        public Entity OccupiedEntity { get; private set; }
        public bool IsOccupied => OccupiedEntity != null;

        [SerializeField] private SpriteRenderer _gridBody;
        [SerializeField] private SpriteRenderer _gridHighlight;
        

        private void Awake()
        {
            Neighbors = new IGridCell[4]; // Up, Down, Left, Right
        }
        
        public void HighlightCell(HighlightType type)
        {
            if(type == HighlightType.None)
            {
                _gridHighlight.color = Color.clear;
            }
            else if(type == HighlightType.Valid)
            {
                _gridHighlight.color = Color.white;
            }
            else if(type == HighlightType.Invalid)
            {
                _gridHighlight.color = Color.grey;
            }
        }
        
        public void SetupCellSize(float size)
        {
            _gridBody.drawMode = SpriteDrawMode.Sliced;
            _gridBody.size = new Vector2(size, size);
            _gridHighlight.drawMode = SpriteDrawMode.Sliced;
            _gridHighlight.size = new Vector2(size - 0.5f, size - 0.5f);
        }

        public void SetNeighbor(IGridCell neighbor, Direction direction)
        {
            Neighbors[(int)direction] = neighbor;
        }
        
        public void SetEntity(Entity entity)
        {
            OccupiedEntity = entity;
            Debug.Log($"Entity {entity.name} placed on cell {name}");
            Debug.Log(IsOccupied);
        }

        public void ClearEntity()
        {
            var entity = OccupiedEntity;
            OccupiedEntity = null;
            
            Destroy(entity.gameObject);
        }
    }
    
    public interface IGridCell
    {
        public GameObject GameObject { get; }
        public IGridCell[] Neighbors { get; }
        public Entity OccupiedEntity { get; }
        public bool IsOccupied { get; }
        
        public void HighlightCell(HighlightType type);
        public void SetNeighbor(IGridCell neighbor, Direction direction);
        public void SetupCellSize(float size);
        public void SetEntity(Entity entity);
        public void ClearEntity();
    }
    
    public enum HighlightType
    {
        None,
        Valid,
        Invalid,
    }
}