using System;
using UnityEngine;

namespace Common
{
    // reference : https://stackoverflow.com/questions/59560530/convert-liste-of-vector3-to-json
    [Serializable]
    public class SerializedVector3
    {
        public double x;
        public double y;
        public double z;

        public SerializedVector3() {}
        public SerializedVector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public SerializedVector3(Vector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3((float)x, (float)y, (float)z);
        }
    }

    [Serializable]
    public class SerializedQuaternion
    {
        public double w;
        public double x;
        public double y;
        public double z;

        public SerializedQuaternion() {}

        public SerializedQuaternion(double w, double x, double y, double z)
        {
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public SerializedQuaternion(Quaternion quaternion)
        {
            w = quaternion.w;
            x = quaternion.x;
            y = quaternion.y;
            z = quaternion.z;
        }

        public Quaternion ToQuaternion()
        {
            return new Quaternion((float)x, (float)y, (float)z, (float)w);
        }
    }
}