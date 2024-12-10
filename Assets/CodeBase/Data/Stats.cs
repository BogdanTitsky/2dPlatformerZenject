using System;

namespace CodeBase.Data
{
    [Serializable]
    public class Stats
    {
        public float MaxHp = 100;
        public float Damage  = 10;
        public float JumpPower = 15f;
        public Vector2Data AttackDistance = new(1.4f, 0.8f);
        public float AttackCleavage = 1.6f;
        public float CurrentHp;
        public float BlockStamina = 100;
        public float BlockStaminaRegenPerSec = 70;

        public void ResetHp() => CurrentHp = MaxHp;

    }
}