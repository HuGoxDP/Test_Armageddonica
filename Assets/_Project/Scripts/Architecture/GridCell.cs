using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace _Project.Scripts.Architecture
{
    public class GridCell: MonoBehaviour
    {
        public Entity OccupiedEntity { get; private set; }
        public bool IsOccupied => OccupiedEntity != null;
        
        public void SetEntity(Entity entity)
        {
            OccupiedEntity = entity;
        }
        
        public void ClearEntity()
        {
            OccupiedEntity = null;
        }
    }
}