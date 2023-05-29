using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace ET.Client
{
    public class ResComponent: Singleton<ResComponent>,ISingletonUpdate,ISingletonAwake
    {

        public string PackageVersion { get; set; }

        public ResourceDownloaderOperation Downloader { get; set; }

        public Dictionary<string, AssetOperationHandle> AssetsOperationHandles = new Dictionary<string, AssetOperationHandle>(100);

        public Dictionary<string, SubAssetsOperationHandle> SubAssetsOperationHandles = new Dictionary<string, SubAssetsOperationHandle>();

        public Dictionary<string, SceneOperationHandle> SceneOperationHandles = new Dictionary<string, SceneOperationHandle>();

        public Dictionary<string, RawFileOperationHandle> RawFileOperationHandles = new Dictionary<string, RawFileOperationHandle>(100);

        public Dictionary<OperationHandleBase, Action<float>> HandleProgresses = new Dictionary<OperationHandleBase, Action<float>>();

        public Queue<OperationHandleBase> DoneHandleQueue = new Queue<OperationHandleBase>();

        #region 生命周期

        public void Awake()
        {
            
        }

        public void Destroy()
        {
            ForceUnloadAllAssets();
            PackageVersion = string.Empty;
            Downloader = null;
            AssetsOperationHandles.Clear();
            SubAssetsOperationHandles.Clear();
            SceneOperationHandles.Clear();
            RawFileOperationHandles.Clear();
            HandleProgresses.Clear();
            DoneHandleQueue.Clear();
        }
        public void Update()
        {
            foreach (var kv in HandleProgresses)
            {
                OperationHandleBase handle = kv.Key;
                Action<float> progress = kv.Value;

                if (!handle.IsValid)
                {
                    continue;
                }

                if (handle.IsDone)
                {
                    DoneHandleQueue.Enqueue(handle);
                    progress?.Invoke(1);
                    continue;
                }

                progress?.Invoke(handle.Progress);
            }

            while (DoneHandleQueue.Count > 0)
            {
                var handle = DoneHandleQueue.Dequeue();
                HandleProgresses.Remove(handle);
            }
        }

        #endregion
        
        
        #region 热更相关

        public async ETTask<int> UpdateVersionAsync(int timeout = 30)
        {
            var package = YooAssets.GetPackage("DefaultPackage");
            var operation = package.UpdatePackageVersionAsync();

            await operation.GetAwaiter();

            PackageVersion = operation.PackageVersion;
            return 1;
        }

        public async ETTask<int> UpdateManifestAsync()
        {
            var package = YooAssets.GetPackage("DefaultPackage");
            var operation = package.UpdatePackageManifestAsync(PackageVersion);

            await operation.GetAwaiter();

            return 1;
        }

        public int CreateDownloader()
        {
            int downloadingMaxNum = 10;
            int failedTryAgain = 3;
            ResourceDownloaderOperation downloader = YooAssets.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            if (downloader.TotalDownloadCount == 0)
            {
                Log.Info("没有发现需要下载的资源");
            }
            else
            {
                Log.Info("一共发现了{0}个资源需要更新下载。".Fmt(downloader.TotalDownloadCount));
                Downloader = downloader;
            }

            return 1;
        }

        public async ETTask<int> DonwloadWebFilesAsync(
        DownloaderOperation.OnStartDownloadFile onStartDownloadFileCallback = null,
        DownloaderOperation.OnDownloadProgress onDownloadProgress = null,
        DownloaderOperation.OnDownloadError onDownloadError = null,
        DownloaderOperation.OnDownloadOver onDownloadOver = null)
        {
            if (Downloader == null)
            {
                return 1;
            }

            // 注册下载回调
            Downloader.OnStartDownloadFileCallback = onStartDownloadFileCallback;
            Downloader.OnDownloadProgressCallback = onDownloadProgress;
            Downloader.OnDownloadErrorCallback = onDownloadError;
            Downloader.OnDownloadOverCallback = onDownloadOver;
            Downloader.BeginDownload();
            await Downloader.GetAwaiter();

            return 1;
        }

        #endregion

        #region 卸载

        public void UnloadUnusedAssets()
        {
            ResourcePackage package = YooAssets.GetPackage("DefaultPackage");
            package.UnloadUnusedAssets();
        }

        public void ForceUnloadAllAssets()
        {
            ResourcePackage package = YooAssets.GetPackage("DefaultPackage");
            package.ForceUnloadAllAssets();
        }

        public void UnloadAsset(string location)
        {
            if (AssetsOperationHandles.TryGetValue(location, out AssetOperationHandle assetOperationHandle))
            {
                assetOperationHandle.Release();
                AssetsOperationHandles.Remove(location);
            }
            else if (RawFileOperationHandles.TryGetValue(location, out RawFileOperationHandle rawFileOperationHandle))
            {
                rawFileOperationHandle.Release();
                RawFileOperationHandles.Remove(location);
            }
            else if (SubAssetsOperationHandles.TryGetValue(location, out SubAssetsOperationHandle subAssetsOperationHandle))
            {
                subAssetsOperationHandle.Release();
                SubAssetsOperationHandles.Remove(location);
            }
            else
            {
                Log.Error($"资源{location}不存在");
            }
        }

        #endregion

        #region 异步加载

        public async ETTask<T> LoadAssetAsync<T>(string location) where T : UnityEngine.Object
        {
            AssetsOperationHandles.TryGetValue(location, out AssetOperationHandle handle);

            if (handle == null)
            {
                handle = YooAssets.LoadAssetAsync<T>(location);
                AssetsOperationHandles[location] = handle;
            }

            await handle;

            return handle.GetAssetObject<T>();
        }

        public async ETTask<UnityEngine.Object> LoadAssetAsync(string location, Type type)
        {
            if (!AssetsOperationHandles.TryGetValue(location, out AssetOperationHandle handle))
            {
                handle = YooAssets.LoadAssetAsync(location, type);
                AssetsOperationHandles[location] = handle;
            }

            await handle;

            return handle.AssetObject;
        }

        public async ETTask<UnityEngine.SceneManagement.Scene> LoadSceneAsync(string location, Action<float> progressCallback = null,
        LoadSceneMode loadSceneMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
        {
            if (!SceneOperationHandles.TryGetValue(location, out SceneOperationHandle handle))
            {
                handle = YooAssets.LoadSceneAsync(location, loadSceneMode, activateOnLoad, priority);
                SceneOperationHandles[location] = handle;
            }

            if (progressCallback != null)
            {
                HandleProgresses.Add(handle, progressCallback);
            }

            await handle;

            return handle.SceneObject;
        }

        public async ETTask<byte[]> LoadRawFileDataAsync(string location)
        {
            if (!RawFileOperationHandles.TryGetValue(location, out RawFileOperationHandle handle))
            {
                handle = YooAssets.LoadRawFileAsync(location);
                RawFileOperationHandles[location] = handle;
            }

            await handle;

            return handle.GetRawFileData();
        }

        public async ETTask<string> LoadRawFileTextAsync(string location)
        {
            if (!RawFileOperationHandles.TryGetValue(location, out RawFileOperationHandle handle))
            {
                handle = YooAssets.LoadRawFileAsync(location);
                RawFileOperationHandles[location] = handle;
            }

            await handle;

            return handle.GetRawFileText();
        }

        #endregion

        #region 同步加载

        public T LoadAsset<T>(string location) where T : UnityEngine.Object
        {
            AssetsOperationHandles.TryGetValue(location, out AssetOperationHandle handle);

            if (handle == null)
            {
                handle = YooAssets.LoadAssetSync<T>(location);
                AssetsOperationHandles[location] = handle;
            }

            return handle.AssetObject as T;
        }

        public UnityEngine.Object LoadAsset(string location, Type type)
        {
            AssetsOperationHandles.TryGetValue(location, out AssetOperationHandle handle);

            if (handle == null)
            {
                handle = YooAssets.LoadAssetSync(location, type);
                AssetsOperationHandles[location] = handle;
            }

            return handle.AssetObject;
        }

        public byte[] LoadRawFileDataSync(string location)
        {
            if (!RawFileOperationHandles.TryGetValue(location, out RawFileOperationHandle handle))
            {
                handle = YooAssets.LoadRawFileSync(location);
                RawFileOperationHandles[location] = handle;
            }

            return handle.GetRawFileData();
        }

        #endregion


    }
}