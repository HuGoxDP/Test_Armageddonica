using System;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Cards.Deck;
using _Project.Scripts.Architecture.Grid.Core;
using _Project.Scripts.Architecture.Hand;
using UnityEngine;

namespace _Project.Scripts.Architecture.Core.GameStates
{
    public class MatchController : MonoBehaviour
    {
        public event EventHandler<GameState> OnGameStateChanged;
        
        [SerializeField] private GridSystem _gridSystem;
        [SerializeField] private PlayerHand _playerHolder;
        [SerializeField] private CardDeck _cardDeck;

        private GameState _state;

        private void Start()
        {
            UpdateGameState(GameState.Initializing);
        }
        private void InitializeGame()
        {
            _playerHolder.SetMatchController(this);
            _gridSystem.SetMatchController(this);
        }
        
        public void UpdateGameState(GameState newState)
        {
            _state = newState;

            switch (_state)
            {
                case GameState.Initializing:
                    Debug.Log("Initializing Game");
                    InitializeGame();
                    NextTurn();
                    break;
                case GameState.CardSelectionTurn:
                    //TODO: Implement card selection logic
                    Debug.Log("Card Selection Turn");
                    AddCardToPlayerHand();
                    NextTurn();
                    break;
                case GameState.CardPlacementTurn:
                    Debug.Log("Card Placement Turn");
                    break;
                case GameState.BuffTurn:
                    //TODO: Implement buff logic
                    //TODO Recalculate army strength
                    Debug.Log("Buff Turn");
                    NextTurn();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            OnGameStateChanged?.Invoke(this, _state);
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

        private void AddCardToPlayerHand()
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