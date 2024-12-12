using System;

namespace CodeBase.Data
{
    [Serializable]
    public class Stats
    {
        public float MaxHp = 100;
        public float Damage  = 3;
        public float JumpPower = 15f;
        public float CurrentHp;
        public float BlockStamina = 100;
        public float BlockStaminaRegenPerSec = 70;

        public void ResetHp() => CurrentHp = MaxHp;
    }
}