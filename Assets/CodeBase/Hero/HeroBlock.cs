using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Input;
using CodeBase.UI.Elements;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
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
        private float _currentStamina;
        private float _maxBlockStamina;
        private float _blockStaminaRegenPerSec;
        private const float BlockConsumptionPerSec = 36;
        private bool _isBlockBtnDown;

        private bool IsBlockBtnDown
        {
            get => _isBlockBtnDown;
            set
            {
                if (_isBlockBtnDown != value)
                {
                    _isBlockBtnDown = value;
                    OnBlockBtnDownChanged();
                }
            }
        }

        private void Update()
        {
            HandleInput();
            UpdateHpBar();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _maxBlockStamina = progress.HeroStats.BlockStamina;
            _currentStamina = _maxBlockStamina;
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
            if (IsBlockBtnDown && _currentStamina >= 0)
            {
                _currentStamina -= BlockConsumptionPerSec * Time.deltaTime;
                blockBar.SetValue(_currentStamina, _maxBlockStamina);
            }
            else if (_currentStamina < _maxBlockStamina)
            {
                IsBlockBtnDown = false;
                _currentStamina += _blockStaminaRegenPerSec * Time.deltaTime;
                blockBar.SetValue(_currentStamina, _maxBlockStamina);
                if (_currentStamina >= _maxBlockStamina)
                {
                    _currentStamina = _maxBlockStamina;
                    blockBar.gameObject.SetActive(false);
                }
            }
        }

        private void HandleInput()
        {
            if (_inputService.IsBlockButtonUp()) IsBlockBtnDown = false;
            if (_inputService.IsBlockButtonDown()) IsBlockBtnDown = true;
        }
    }
}