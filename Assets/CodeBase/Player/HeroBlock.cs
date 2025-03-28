﻿using System.Collections;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.UI.Elements;
using UnityEngine;

namespace CodeBase.Player
{
    /// <summary>
    /// This class represents the hero's block functionality. It handles the hero's block stamina,
    /// block bar UI, and block animation. It also loads the hero's block stamina progress from the player's save data.
    /// </summary>
    public class HeroBlock : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private HeroAnimator _heroAnimator;
        [SerializeField] private BlockBar blockBar;



        private const float BlockConsumptionPerSec = 36;
        private float _maxBlockStamina;
        private float _blockStaminaRegenPerSec;
        private bool _blockBroken;
        
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
        public void BlockDamage(float damage)
        {
            damage *= 3;
            if (damage > CurrentStamina) StartCoroutine(BlockBreakerRecoveryRoutine());
            CurrentStamina -= damage;
        }

        private void Update()
        {
            UpdateBlockBar();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _maxBlockStamina = progress.HeroStats.BlockStamina;
            CurrentStamina = _maxBlockStamina;
            _blockStaminaRegenPerSec = progress.HeroStats.BlockStaminaRegenPerSec;
            HideBar();
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

        private void UpdateBlockBar()
        {
            if (IsBlockBtnDown && CurrentStamina >= 0)
                CurrentStamina -= BlockConsumptionPerSec * Time.deltaTime;
            else if (CurrentStamina < _maxBlockStamina)
            {
                CurrentStamina += _blockStaminaRegenPerSec * Time.deltaTime;
                if (CurrentStamina >= _maxBlockStamina)
                {
                    CurrentStamina = _maxBlockStamina;
                    HideBar();
                }
            }
            blockBar.SetValue(CurrentStamina, _maxBlockStamina);
        }

        private void HideBar() => 
            blockBar.gameObject.SetActive(false);

        private IEnumerator BlockBreakerRecoveryRoutine()
        {
            _blockBroken = true;
            _heroAnimator.IsStunnedOn();
            yield return new WaitForSeconds(_maxBlockStamina / _blockStaminaRegenPerSec);
            _heroAnimator.IsStunnedOff();
            _blockBroken = false;
        }
    }
}