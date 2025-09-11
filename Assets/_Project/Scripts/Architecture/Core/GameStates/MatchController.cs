using System;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Cards.Deck;
using _Project.Scripts.Architecture.EffectApplicators;
using _Project.Scripts.Architecture.Enums;
using _Project.Scripts.Architecture.Grid.Core;
using _Project.Scripts.Architecture.Hand;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.GameStates
{
    public class MatchController : MonoBehaviour
    {
        public event EventHandler<GameState> OnGameStateChanged;
        
        [SerializeField] private GridSystem _gridSystem;
        [SerializeField] private PlayerHand _playerHolder;
        [SerializeField] private EntityEffectManager _entityEffectManager;
        [SerializeField] private CardDeck _cardDeck;

        private GameState _state;
        private int _turnNumber;

        private void Start()
        {
            UpdateGameState(GameState.Initializing);
        }
        private void InitializeGame()
        {
            _playerHolder.SetMatchController(this);
            _gridSystem.SetMatchController(this);
            _entityEffectManager.SetMatchController(this);
        }
        
        public void UpdateGameState(GameState newState)
        {
            _state = newState;

            switch (_state)
            {
                case GameState.Initializing:
                    InitializeGame();
                    NextTurn();
                    break;
                case GameState.CardSelectionTurn:
                    //TODO: Implement card selection logic
                    _turnNumber++;
                    AddCardsToPlayerHand(_turnNumber == 1 ? 3 : 1); 
                    NextTurn();
                    break;
                case GameState.CardPlacementTurn:
                    break;
                case GameState.BuffTurn:
                    BuffTurn();
                    //TODO Recalculate army strength
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            OnGameStateChanged?.Invoke(this, _state);
        }

        private async void BuffTurn()
        {
            try
            {
                await _entityEffectManager.CalculateAllEffects();
                RecalculateArmyStrength();
                NextTurn();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void NextTurn()
        {
            switch (_state)
            {
                case GameState.Initializing:
                    UpdateGameState(GameState.CardSelectionTurn);
                    break;
                case GameState.CardSelectionTurn:
                    UpdateGameState(GameState.CardPlacementTurn);
                    break;
                case GameState.CardPlacementTurn:
                    UpdateGameState(GameState.BuffTurn);
                    break;
                case GameState.BuffTurn:
                    UpdateGameState(GameState.CardSelectionTurn);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void RecalculateArmyStrength()
        {
            // TODO: Перерахувати загальну силу армії
        }
        
        [Button("TEST Add Card To Hand")]
        private void TestAddCardToHand() => AddSingleCardToPlayerHand();
        
        private void AddCardsToPlayerHand(int numberOfCards)
        {
            if (_cardDeck == null || _playerHolder == null) return;
            
            for (int i = 0; i < numberOfCards; i++)
            {
                AddSingleCardToPlayerHand();
            }
        }
        
        private void AddSingleCardToPlayerHand()
        {
            if (_cardDeck == null || _playerHolder == null) return;
            
            var cardData = GetRandomCardFromDeck(_cardDeck);
            _playerHolder.AddCard(cardData);
        }

        private BaseCardData GetRandomCardFromDeck(CardDeck cardDeck)
        {
            if (cardDeck.Cards.Count == 0)
            {
                Debug.LogWarning("Card deck is empty.");
                return null;
            }

            var randomIndex = UnityEngine.Random.Range(0, cardDeck.Cards.Count);
            return cardDeck.Cards[randomIndex];
        }
    }
}