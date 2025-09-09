using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Entities.Base;
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.Grid.Core
{
    public class GridCell : MonoBehaviour, IGridCell
    {
        public GameObject GameObject => gameObject;
        public Entity OccupiedEntity { get; private set; }
        public bool IsOccupied => OccupiedEntity != null;
        public Vector2Int Position { get; private set; }
        
        [SerializeField] private SpriteRenderer _gridBody;
        [SerializeField] private SpriteRenderer _gridHighlight;
        
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
        }

        public void Initialize(float cellSize, Vector2Int position)
        {
            Position = position;
            
            _gridBody.drawMode = SpriteDrawMode.Sliced;
            _gridBody.size = new Vector2(cellSize, cellSize);
            _gridHighlight.drawMode = SpriteDrawMode.Sliced;
            _gridHighlight.size = new Vector2(cellSize - 0.5f, cellSize - 0.5f);
        }
        
        public void SetEntity(Entity entity)
        {
            OccupiedEntity = entity;
            entity.InitializePosition(Position);
        }

        public void ClearEntity()
        {
            var entity = OccupiedEntity;
            OccupiedEntity = null;
            
            Destroy(entity.gameObject);
        }
    }
}