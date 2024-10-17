using System.Collections.Generic;
using UnityEngine;
using Common;

namespace Map
{
    public class MapData
    {
        public string themeId;
        public class MapObjectData
        {
            public string modelId;
            public SerializedVector3 position;
            public SerializedQuaternion rotation;
        }
        public List<MapObjectData> mapObjectList;
    }
    
}