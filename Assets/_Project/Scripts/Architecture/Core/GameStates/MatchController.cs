using System;
using _Project.Scripts.Architecture.Cards.Data;
using _Project.Scripts.Architecture.Cards.Deck;
using _Project.Scripts.Architecture.Core.Interfaces;
using _Project.Scripts.Architecture.EffectApplicators;
using _Project.Scripts.Architecture.Enums;
using _Project.Scripts.Architecture.Grid.Core;
using _Project.Scripts.Architecture.Hand;
using _Project.Scripts.Architecture.SelectCardMenu;
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
        [SerializeField] private SelectCardMenu.SelectCardMenu _selectCardMenu;
        [SerializeField] private ArmyStatUI _armyStatUI;
        
        private ICardGeneratorStrategy _generatorStrategy;
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
            _selectCardMenu.SetMatchController(this);
            _generatorStrategy ??= new CardGeneratorStrategy();
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
                    GenerateCards();
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

        private void GenerateCards()
        {
            if (_turnNumber == 1)
            {
               var cards = _generatorStrategy.GenerateCards(_selectCardMenu.CardDeck, 3);
               foreach (var card in cards)
               {
                   _playerHolder.AddCard(card);
               }
               NextTurn();
            }
            else
            {
                _selectCardMenu.gameObject.SetActive(true);
                _selectCardMenu.GenerateCards(_generatorStrategy, 3);   
            }
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
            _armyStatUI.CalculateStrength(_gridSystem);
        }
    }
}