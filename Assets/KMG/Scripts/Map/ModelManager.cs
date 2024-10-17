using System;
using System.Collections.Generic;
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

        private ModelManager()
        {
            if (_instance != null) return;
            Init();
        }
        
        #region init
        public void Init()
        {
            downloadList = new List<Model>();
            LoadDownloadList();
        }
        
        private void LoadDownloadList()
        {
            // load default assets
            downloadList.AddRange(Resources.FindObjectsOfTypeAll<Model>());
            
            // load downloaded assets
            // throw new NotImplementedException();
        }
        #endregion
        
        #region find
        public Model Find(MapData.MapObjectData mapObjectData)
        {
            return Find(mapObjectData.modelId);
        }

        public Model Find(string modelId)
        {
            Model model = downloadList.Find((e) => e.id == modelId);
            if (model == null) model = Download(model.id);
            return model;
        }

        /**
         * Find 중 모델이 로컬에 없으면 다운로드하기
         */
        private Model Download(string modelId)
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