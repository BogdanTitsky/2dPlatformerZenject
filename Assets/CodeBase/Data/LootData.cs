using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LootData
    {
        public int Collected;
        public Action OnChange;

        public void Collect(Loot loot)
        {
            Collected += loot.Value;
            OnChange?.Invoke();
        }
    }
}