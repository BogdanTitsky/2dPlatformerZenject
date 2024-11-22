using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Input;
using CodeBase.UI.Elements;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    /// <summary>
    /// This class represents the hero's block functionality. It handles the hero's block stamina,
    /// block bar UI, and block animation. It also loads the hero's block stamina progress from the player's save data.
    /// </summary>
    public class HeroBlock : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private HeroAnimator _heroAnimator;
        [SerializeField] private BlockBar blockBar;

        [Inject]
        public void Init(IInputService inputService)
        {
            _inputService = inputService;
        }

        private IInputService _inputService;

        private const float BlockConsumptionPerSec = 36;
        private float _maxBlockStamina;
        private float _blockStaminaRegenPerSec;
        private bool _isBlockBtnDown;
        private float _currentStamina;

        /// <summary>
        /// Gets or sets a value indicating whether the block button is currently down.
        /// </summary>
        public bool IsBlockBtnDown
        {
            get => _isBlockBtnDown;
            private set
            {
                if (_isBlockBtnDown != value)
                {
                    _isBlockBtnDown = value;
                    OnBlockBtnDownChanged();
                }
            }
        }

        private float CurrentStamina
        {
            get => _currentStamina;
            set
            {
                _currentStamina = value;
                if (_currentStamina <=0)
                {
                    IsBlockBtnDown = false;
                    _currentStamina = 0;
                }
            }
        }

        /// <summary>
        /// Reduces the hero's block stamina by the given damage amount.
        /// </summary>
        /// <param name="damage">The amount of damage to reduce the block stamina by.</param>
        public void BlockDamage(float damage) => CurrentStamina -= damage;

        private void Update()
        {
            HandleInput();
            UpdateHpBar();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _maxBlockStamina = progress.HeroStats.BlockStamina;
            CurrentStamina = _maxBlockStamina;
            _blockStaminaRegenPerSec = progress.HeroStats.BlockStaminaRegenPerSec;
        }

        private void OnBlockBtnDownChanged()
        {
            if (IsBlockBtnDown)
            {
                blockBar.gameObject.SetActive(true);
                _heroAnimator.IsBlockingOn();
            }
            else
                _heroAnimator.IsBlockingOff();
        }

        private void UpdateHpBar()
        {
            if (IsBlockBtnDown && CurrentStamina >= 0)
                CurrentStamina -= BlockConsumptionPerSec * Time.deltaTime;
            else if (CurrentStamina < _maxBlockStamina)
            {
                CurrentStamina += _blockStaminaRegenPerSec * Time.deltaTime;
                if (CurrentStamina >= _maxBlockStamina)
                {
                    CurrentStamina = _maxBlockStamina;
                    blockBar.gameObject.SetActive(false);
                }
            }
            blockBar.SetValue(CurrentStamina, _maxBlockStamina);
        }

        private void HandleInput()
        {
            if (_inputService.IsBlockButtonUp()) IsBlockBtnDown = false;
            if (_inputService.IsBlockButtonDown()) IsBlockBtnDown = true;
        }
    }
}