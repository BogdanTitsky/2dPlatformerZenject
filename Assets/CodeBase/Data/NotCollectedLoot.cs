using System;
using System.Collections.Generic;

namespace CodeBase.Data
{
    [Serializable]
    public class NotCollectedLoot
    { 
        public List<Loot> NotCollectedList = new List<Loot>();
    }
}