using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class MapData
    {
        public string mapThemeId;
        public class MapObjectData
        {
            public string modelId;
            public Vector3 position;
            public Quaternion rotation;
        }
        public List<MapObjectData> mapObjectList;
    }
    
}