using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Map
{
    /**
     * 모델 에셋 저장 및 다운로드 관리
     * TODO: Download, Import, Upload 구현
     */
    public class ModelManager
    {
        private static ModelManager _instance;
        public static ModelManager Instance
        {
            get
            {
                if (_instance == null) _instance = new ModelManager();
                return _instance;
            }
        }
        private List<Model> downloadModelList;
        private List<Theme> downloadThemeList;
        private bool isLoaded;

        private ModelManager()
        {
            if (_instance != null) return;
            Init();
        }
        
        #region init
        public void Init()
        {
            LoadDownloadList().Forget();
        }
        
        /**
         * TODO: downloaded assets 로드 구현하기
         */
        private async UniTaskVoid LoadDownloadList()
        {
            isLoaded = false;
            downloadModelList = new List<Model>();
            downloadThemeList = new List<Theme>();
            
            // load default assets
            downloadModelList.AddRange(await LoadDefaultModelList());
            downloadThemeList.AddRange(await LoadDefaultThemeList());
            
            // load downloaded assets
            // ...

            isLoaded = true;
        }

        private async UniTask<Model[]> LoadDefaultModelList()
        {
            string[] modelNames = new[] { "Cube", "Sphere" };
            List<UniTask<Model>> tasks = new List<UniTask<Model>>();
            foreach (var modelName in modelNames)
            {
                tasks.Add(LoadModelFromResources(modelName));
            }

            Model[] assets = await UniTask.WhenAll(tasks);
            return assets;
        }
        
        private async UniTask<Theme[]> LoadDefaultThemeList()
        {
            string[] themeNames = new[] { "Theme0", "Theme1", "Theme2" };
            List<UniTask<Theme>> tasks = new List<UniTask<Theme>>();
            foreach (var themeName in themeNames)
            {
                tasks.Add(LoadThemeFromResources(themeName));
            }

            Theme[] assets = await UniTask.WhenAll(tasks);
            return assets;
        }

        private async UniTask<Model> LoadModelFromResources(string assetName)
        {
            // reference : https://discussions.unity.com/t/unitask-asyncload-list-of-resources/895491
            var model = await Resources.LoadAsync<Model>("Models/" + assetName);
            return model as Model;
        }
        
        private async UniTask<Theme> LoadThemeFromResources(string assetName)
        {
            // reference : https://discussions.unity.com/t/unitask-asyncload-list-of-resources/895491
            var Theme = await Resources.LoadAsync<Theme>("Themes/" + assetName);
            return Theme as Theme;
        }
        #endregion
        
        
        #region get
        /**
         * Model 데이터를 반환
         */
        public async UniTask<Model> Get(MapData.MapObjectData mapObjectData)
        {
            return await Get(mapObjectData.modelId);
        }

        public async UniTask<Model> Get(string modelId)
        {
            await UniTask.WaitUntil(() => isLoaded == true);
            Model model = downloadModelList.Find((e) => e.id == modelId);
            if (model == null) model = await Download(modelId);
            return model;
        }
        
        public async UniTask<Theme> GetTheme(string themeId)
        {
            await UniTask.WaitUntil(() => isLoaded == true);
            Debug.Log(downloadThemeList[0]);
            Theme theme = downloadThemeList.Find((e) => e.id == themeId);
            if (theme == null) theme = await DownloadTheme(themeId);
            return theme;
        }

        /**
         * Find 중 모델이 로컬에 없으면 다운로드하기
         * 에셋 참고: https://assetstore.unity.com/packages/tools/modeling/trilib-2-model-loading-package-157548
         */
        private async UniTask<Model> Download(string modelId)
        {
            throw new NotImplementedException();
        }
        
        private async UniTask<Theme> DownloadTheme(string themeId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region import
        /**
         * 로컬에서 모델 새로 찾아 로드하기
         * 에셋 참고: https://assetstore.unity.com/packages/tools/modeling/trilib-2-model-loading-package-157548
         */
        public Model Import()
        {
            throw new NotImplementedException();
        }

        /**
         * Import 중에 모델 Upload하는 과정
         */
        private void Upload(Model model)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}