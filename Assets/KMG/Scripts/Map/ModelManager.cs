using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Map
{
    /**
     * 모델 에셋 저장 및 다운로드 관리
     * TODO: Find, LoadDownloadList 구현
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
        private List<Model> downloadList;
        private bool isLoaded;

        private ModelManager()
        {
            if (_instance != null) return;
            Init();
        }
        
        #region init
        public void Init()
        {
            downloadList = new List<Model>();
            LoadDownloadList().Forget();
        }
        
        private async UniTaskVoid LoadDownloadList()
        {
            // load default assets
            string[] assetNames = new[] { "Cube", "Sphere" };
            List<UniTask<Model>> tasks = new List<UniTask<Model>>();
            foreach (var assetName in assetNames)
            {
                tasks.Add(LoadModel(assetName));
            }

            Model[] assets = await UniTask.WhenAll(tasks);
            downloadList.AddRange(assets);
            downloadList.ForEach((e)=>Debug.Log(e));
            
            // load downloaded assets
            // throw new NotImplementedException();

            isLoaded = true;
        }

        private async UniTask<Model> LoadModel(string assetName)
        {
            // reference : https://discussions.unity.com/t/unitask-asyncload-list-of-resources/895491
            var model = await Resources.LoadAsync<Model>("Models/" + assetName);
            return model as Model;
        }
        #endregion
        
        
        #region find
        public async UniTask<Model> Find(MapData.MapObjectData mapObjectData)
        {
            return await Find(mapObjectData.modelId);
        }

        public async UniTask<Model> Find(string modelId)
        {
            await UniTask.WaitUntil(() => isLoaded == true);
            Model model = downloadList.Find((e) => e.id == modelId);
            if (model == null) model = await Download(modelId);
            return model;
        }

        /**
         * Find 중 모델이 로컬에 없으면 다운로드하기
         */
        private async UniTask<Model> Download(string modelId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region import
        /**
         * 로컬에서 모델 새로 찾아 로드하기
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