using _Project.Scripts.Architecture.Entities.Base;
using UnityEngine;

namespace _Project.Scripts.Architecture.Grid.Core
{
    public class GridCell : MonoBehaviour
    {
        public Entity OccupiedEntity { get; private set; }
        public bool IsOccupied => OccupiedEntity != null;

        [SerializeField] private Renderer _renderer;

        public void SetEntity(Entity entity)
        {
            if (IsOccupied)
            {
                Debug.LogWarning("Cell is already occupied.");
                return;
            }

            _renderer.material.color = Color.red; // TODO: Візуально показати зайнятість клітинки
            OccupiedEntity = entity;
        }

        public void ClearEntity()
        {
            // TODO: Візуально показати очищення
            _renderer.material.color = Color.white;
            OccupiedEntity = null;
        }

        public void Highlight(bool highlight)
        {
            // TODO: Підсвітити клітинку при наведенні
            _renderer.material.color = highlight ? Color.yellow : Color.white;
        }
    }
}