using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Cards.Deck;
using _Project.Scripts.Architecture.Enums;
using UnityEngine;

namespace _Project.Scripts.Architecture.SelectCardMenu
{
    public class CardGeneratorStrategy : ICardGeneratorStrategy
    {
        public List<BaseCardData> GenerateCards(CardDeck deck, int count)
        {
            var cards = new List<BaseCardData>();

            if (deck.Cards.Count == 0)
            {
                Debug.LogError("Deck is empty! Cannot generate cards.");
                return cards;
            }

            for (int i = 0; i < count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, deck.Cards.Count);
                var card = deck.Cards[randomIndex];
                cards.Add(card);
            }

            var spellCardCount = cards.Count(card => card.CardType == CardType.Spell);
            var unitCardCount = cards.Count(card => card.CardType == CardType.Unit);
            var buildingCardCount = cards.Count(card => card.CardType == CardType.Building);

            if (spellCardCount > 2)
            {
                BalanceCardType(deck, cards, CardType.Spell, spellCardCount - 2);
            }

            if (unitCardCount > 2)
            {
                BalanceCardType(deck, cards, CardType.Unit, unitCardCount - 2);
            }

            if (buildingCardCount > 2)
            {
                BalanceCardType(deck, cards, CardType.Building, buildingCardCount - 2);
            }

            return cards;
        }

        private void BalanceCardType(CardDeck deck, List<BaseCardData> cards, CardType cardTypeToReduce,
            int countToRemove)
        {
            var cardsToRemove = cards.Where(card => card.CardType == cardTypeToReduce).Take(countToRemove).ToList();

            foreach (var card in cardsToRemove)
            {
                cards.Remove(card);

                if (TryGetRandomCardOfOtherType(deck, cards, cardTypeToReduce, out var replacementCard))
                {
                    cards.Add(replacementCard);
                }
                else
                {
                    Debug.LogWarning(
                        $"Could not find replacement card for type {cardTypeToReduce}. " +
                        $"The generated set will have fewer cards."
                    );
                }
            }
        }

        private bool TryGetRandomCardOfOtherType(CardDeck deck, List<BaseCardData> currentCards, CardType excludedType,
            out BaseCardData card)
        {
            card = null;

            var availableTypes = new List<CardType> { CardType.Spell, CardType.Unit, CardType.Building }
                .Where(type => type != excludedType)
                .ToList();

            availableTypes = availableTypes.OrderBy(x => UnityEngine.Random.value).ToList();

            foreach (var type in availableTypes)
            {
                List<BaseCardData> deckOfType = GetDeckByType(deck, type);

                var availableCards = deckOfType.Where(deckCard => currentCards.All(currentCard => currentCard != deckCard)).ToList();
                
                if (availableCards.Count > 0)
                {
                    // Выбираем случайную карту из доступных
                    card = availableCards[UnityEngine.Random.Range(0, availableCards.Count)];
                    return true;
                }
            }
            
            var allAvailableCards = deck.Cards.Where(deckCard => currentCards.All(currentCard => currentCard != deckCard) && 
                                                                 deckCard.CardType != excludedType).ToList();
            if (allAvailableCards.Count > 0)
            {
                card = allAvailableCards[UnityEngine.Random.Range(0, allAvailableCards.Count)];
                return true;
            }
            
            return false;
        }

        private List<BaseCardData> GetDeckByType(CardDeck deck, CardType type)
        {
            return type switch
            {
                CardType.Spell => deck.SpellCards,
                CardType.Unit => deck.UnitCards,
                CardType.Building => deck.BuildingsCard,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}