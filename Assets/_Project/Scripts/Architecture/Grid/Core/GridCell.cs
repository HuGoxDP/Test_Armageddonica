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
            _gridHighlight.color = type switch
            {
                HighlightType.None => Color.clear,
                HighlightType.Valid => Color.white,
                HighlightType.Hovered => Color.yellow,
                _ => _gridHighlight.color
            };
        }
        public void Initialize(float cellSize, Vector2Int position)
        {
            Position = position;
            
            _gridBody.drawMode = SpriteDrawMode.Sliced;
            _gridBody.size = new Vector2(cellSize + 0.2f, cellSize + 0.2f);
            _gridHighlight.drawMode = SpriteDrawMode.Sliced;
            _gridHighlight.size = new Vector2(cellSize - 1, cellSize - 1);
            
            var boxCollider2D = GetComponent<BoxCollider2D>();
            if (boxCollider2D == null)
            {
                boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            }
            boxCollider2D.size = _gridBody.size;
            boxCollider2D.isTrigger = true;
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
            
            if (entity != null)
                Destroy(entity.gameObject);
        }
        
    }
}