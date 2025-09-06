using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Grid.Core;
using UnityEngine;

namespace _Project.Scripts.Architecture.Hand
{
    /// <summary> Handles the placement of cards onto the grid. </summary>
    public class CardPlacementHandler: MonoBehaviour
    {
        [SerializeField] private GridSystem _gridSystem;

        public bool TryPlaceCard(CardUI eCard, Vector2 eventDataPosition)
        {
            throw new System.NotImplementedException();
        }
    }
}