using System.Threading.Tasks;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.AssetManagement
{
    public interface IAssetProvider : IService
    {
        Task<T> Load<T>(AssetReference enemyDataPrefabReference) where T : class;
        Task<T> Load<T>(string address) where T : class;
        void Cleanup();
        void Initialize();
    }
}