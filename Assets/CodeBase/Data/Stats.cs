using System;

namespace CodeBase.Data
{
    [Serializable]
    public class Stats
    {
        public float MaxHp = 50;
        public float Damage  = 10;
        public float JumpPower = 5f;
        public Vector2Data AttackDistance = new(1.4f, 0.8f);
        public float AttackCleavage = 1.6f;
        public float CurrentHp;

        public void ResetHp() => CurrentHp = MaxHp;

    }
}