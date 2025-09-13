using System;
using System.Collections.Generic;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.Cards.Deck
{
    [CreateAssetMenu(menuName = "Game/CardSystem/New Card Deck")]
    public class CardDeck : ScriptableObject
    {
        [field: SerializeField] public List<BaseCardData> Cards { get; private set; } = new();

        public List<BaseCardData> SpellCards { get; private set; } = new();
        public List<BaseCardData> UnitCards { get; private set; } = new();
        public List<BaseCardData> BuildingsCard { get; private set; } = new();

        private void Awake()
        {
            SortCards();
        }

        private void SortCards()
        {
            foreach (var card in Cards)
            {
                if (card.CardType == CardType.Spell)
                {
                    SpellCards.Add(card);
                }
                else if (card.CardType == CardType.Unit)
                {
                    UnitCards.Add(card);
                }
                else if (card.CardType == CardType.Building)
                {
                    BuildingsCard.Add(card);
                }
            }
        }
    }
}