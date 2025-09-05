using UnityEngine;

namespace _Project.Scripts.Architecture.GameCore
{
    public class GridSystem : MonoBehaviour
    {
        [SerializeField] private int _width = 5;
        [SerializeField] private int _height = 5;
        [SerializeField] private int _cellSize = 1;
        [SerializeField] private GridCell _cellPrefab;
        
        private GridCell[,] _cells;

        private GridCell GetCellAtPosition(Vector3 worldPosition)
        {
            int x = Mathf.
        }
    }
}