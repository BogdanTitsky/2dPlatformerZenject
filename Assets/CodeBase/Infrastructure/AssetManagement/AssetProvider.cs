using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CodeBase.Infrastructure.AssetManagement
{
  public class AssetProvider : IAssetProvider
  {
    private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new();
    private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();

    public void Initialize()
    {
      Addressables.InitializeAsync();
    }
    
    public async Task<T> Load<T>(AssetReference enemyDataPrefabReference) where T : class
    {
      if (_completedCache.TryGetValue(enemyDataPrefabReference.AssetGUID, out AsyncOperationHandle completedHandle))
        return completedHandle.Result as T;

      return await RunWithCacheOnComplete(
        Addressables.LoadAssetAsync<T>(enemyDataPrefabReference), 
        cacheKey: enemyDataPrefabReference.AssetGUID);
    }

    public async Task<T> Load<T>(string address) where T : class
    {
      if (_completedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
        return completedHandle.Result as T;
      
      return await RunWithCacheOnComplete(
        Addressables.LoadAssetAsync<T>(address), 
        cacheKey: address);
    }

    public void Cleanup()
    {
      foreach (List<AsyncOperationHandle> resourceHandles in _handles.Values)
      foreach (AsyncOperationHandle handle in resourceHandles)
        Addressables.Release(handle);
      
      _completedCache.Clear();
      _handles.Clear();
    }

    private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
    {
      handle.Completed += completeHandle =>
      {
        _completedCache[cacheKey] = completeHandle;
      };

      AddHandle<T>(cacheKey, handle);

      return await handle.Task;
    }

    private void AddHandle<T>(string key, AsyncOperationHandle handle) where T : class
    {
      if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandles))
      {
        resourceHandles = new List<AsyncOperationHandle>();
        _handles[key] = resourceHandles;
      }

      resourceHandles.Add(handle);
    }
  }
}