using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Architecture.Cards.Deck;
using _Project.Scripts.Architecture.Cards.Runtime;
using _Project.Scripts.Architecture.Core.Dependency_Injection;
using _Project.Scripts.Architecture.Core.GameStates;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.Enums;
using _Project.Scripts.Architecture.Hand;
using _Project.Scripts.Architecture.Layout;
using _Project.Scripts.Architecture.Tooltip;
using UnityEngine;

namespace _Project.Scripts.Architecture.SelectCardMenu
{
    public class SelectCardMenu : GameControllable, ISelectCardMenu
    {
        [field: SerializeField] public CardDeck CardDeck { get; private set; }
        
        [SerializeField] private LayoutController _layoutController;
        [SerializeField] private PlayerHand _playerHand;

        private List<CardUI> _cards;
        private ICardFactory _cardFactory;
        private CardUI _hoveredCard;
        private bool _isLayoutInitialized = false;

        private void Awake()
        {
            _cards = new List<CardUI>();

            if (_playerHand == null) throw new InvalidOperationException("Player hand is not set");
            if (CardDeck == null) throw new InvalidOperationException("Card deck is not set");
            if (_layoutController == null) throw new InvalidOperationException("Layout controller is not set");
        }

        private void Start()
        {
            _cardFactory ??= ServiceLocator.Get<ICardFactory>();
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            if (_isLayoutInitialized) return;
            
            _layoutController.Initialize(new LinearLayoutStrategy());
            _isLayoutInitialized = true;
        }
        
        public void GenerateCards(ICardGeneratorStrategy cardGeneratorStrategy, int count)
        {
            if (cardGeneratorStrategy == null)
            {
                throw new ArgumentNullException(nameof(cardGeneratorStrategy), "Card generator strategy cannot be null");
            }
            
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than or equal to 0");
            }

            if (!_isLayoutInitialized)
            {
                InitializeLayout();
            }
            
            var cards = cardGeneratorStrategy.GenerateCards(CardDeck, count);
            _cardFactory ??= ServiceLocator.Get<ICardFactory>();

            RemoveAllCards();
            
            foreach (var cardData in cards)
            {
                var card = _cardFactory.CreateCard(cardData, _layoutController.transform);
                AddCard(card);
            }

            StartCoroutine(UpdateLayoutWithDelay());
        }

        private void AddCard(CardUI card)
        {
            _cards.Add(card);
            var cardInteractionHandler = card.GetComponent<CardEventHandler>();
            if (cardInteractionHandler != null)
            {
                cardInteractionHandler.OnCardClicked += OnCardSelected;
                cardInteractionHandler.OnCardHovered += OnCardHovered;
            }

            _layoutController.AddCard(card.transform);
        }
        
        private void RemoveCard(CardUI card)
        {
            if (card == null) return;

            var cardInteractionHandler = card.GetComponent<CardEventHandler>();
            if (cardInteractionHandler != null)
            {
                cardInteractionHandler.OnCardClicked -= OnCardSelected;
                cardInteractionHandler.OnCardHovered -= OnCardHovered;
            }

            _layoutController.RemoveCard(card.transform);
            Destroy(card.gameObject);
        }

        private void RemoveAllCards()
        {
            var cardsToRemove = new List<CardUI>(_cards);

            foreach (var card in cardsToRemove)
            {
                RemoveCard(card);
            }

            _cards.Clear();
            _hoveredCard = null;
        }

        private void OnCardHovered(object sender, CardUI card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card), "Card cannot be null");
            if (_hoveredCard != null && _hoveredCard == card)
                return;

            _hoveredCard = card;
            _layoutController.UpdateHoverLayout(_cards.IndexOf(card));

            // make card bigger 
            // other cards smaller and darker
        }

        private void OnCardSelected(object sender, CardUI card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card), "Card cannot be null");
            }

            _hoveredCard = null;
            _playerHand.AddCard(card.CardData);
            RemoveAllCards();

            MatchController.NextTurn();
        }

        private IEnumerator UpdateLayoutWithDelay()
        {
            yield return null;
            _layoutController.UpdateHoverLayout(0);
        }
        
        protected override void OnGameStateChanged(object sender, GameState newState)
        {
            gameObject.SetActive(newState == GameState.CardSelectionTurn);
            
            if (newState == GameState.CardSelectionTurn && !_isLayoutInitialized)
            {
                InitializeLayout();
            }
        }
    }
}