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
            // public Vector3 position;
            public SerializedVector3 position;
            // public Quaternion rotation;
            public SerializedQuaternion rotation;
        }
        public List<MapObjectData> mapObjectList;
    }
    
}