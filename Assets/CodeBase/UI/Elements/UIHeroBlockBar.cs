using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.UI.Elements;
using UnityEngine;

public class UIHeroBlockBar : MonoBehaviour, ISavedProgressReader
{
    [SerializeField] private BlockBar blockBar;

    private bool _isBlockActive;
    public bool IsBlockActive {
        get => _isBlockActive;
        set{
        if (_isBlockActive != value)
        {
            _isBlockActive = value;
            OnBlockActiveChanged();
        }
        } }

    

    private float _maxBlockStamina;
    private float _currentStamina;
    private  float _blockStaminaRegenPerSec;

    private const float BlockConsumptionPerSec = 36;

    private void Update() => 
        UpdateHpBar();

    public void LoadProgress(PlayerProgress progress)
    {
        _maxBlockStamina = progress.HeroStats.BlockStamina;
        _currentStamina = _maxBlockStamina / 10;
        _blockStaminaRegenPerSec = progress.HeroStats.BlockStaminaRegenPerSec;
    }
    
    private void UpdateHpBar()
    {
        if (IsBlockActive && _currentStamina >= 0)
        {
            _currentStamina -= BlockConsumptionPerSec * Time.deltaTime;
            blockBar.SetValue(_currentStamina, _maxBlockStamina);
        }
        else if (_currentStamina < _maxBlockStamina)
        {
            _currentStamina += _blockStaminaRegenPerSec * Time.deltaTime;
            blockBar.SetValue(_currentStamina, _maxBlockStamina);
            if (_currentStamina >= _maxBlockStamina)
            {
                _currentStamina = _maxBlockStamina;
                blockBar.gameObject.SetActive(false);
            }
        }
    }
    
    private void OnBlockActiveChanged()
    {
        if (IsBlockActive)
            blockBar.gameObject.SetActive(true);
    }
    
}
