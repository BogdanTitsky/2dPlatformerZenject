using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.StaticData.Data.Windows
{
    [CreateAssetMenu(fileName = "WindowStaticData", menuName = "StaticData/WindowStatic")]

    public class WindowStaticData : ScriptableObject
    {
        public List<WindowConfig> Configs;
    }
}