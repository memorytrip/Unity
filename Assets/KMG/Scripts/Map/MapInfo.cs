using System;
using UnityEngine;

namespace Map
{
    public class MapInfo
    {
        public string id;
        public Texture2D thumbnail;
        public string data;

        public MapConcrete ConvertToMapConcrete()
        {
            throw new NotImplementedException();
        }
    }
}