using UnityEngine;
using System;
using System.Collections;
using Astra;

namespace Astra.Simple
{
    public class DataHelper
    {
        public static Vector3D Orientation2Vector3D(Matrix3x3 matrix)
        {
            return new Vector3D();
        }

        public static Quaternion Orientation2Quaternion(Matrix3x3 matrix)
        {
            return new Quaternion();
        }


//        private static float mapConvert_fXToZ;
//        private static float mapConvert_fYToZ;
//        private static float mapConvert_fCoeffX;
//        private static float mapConvert_fCoeffY;
//        private static float mapConvert_resolutionX;
//        private static float mapConvert_resolutionY;
//        private static float mapConvert_nHalfXres;
//        private static float mapConvert_nHalfYres;
//        private static bool mapConvert_isCalculated = false;
//
//        static DataHelper()
//        {
//            mapConvert_fXToZ = 2f * (float)Math.Tan((double)(1.0226f / 2f));
//            mapConvert_fYToZ = 2f * (float)Math.Tan((double)(0.7966157f / 2f));
//            mapConvert_resolutionX = 1f;
//            mapConvert_resolutionY = 1f;
//            mapConvert_nHalfXres = mapConvert_resolutionX / 2f;
//            mapConvert_nHalfYres = mapConvert_resolutionY / 2f;
//            mapConvert_fCoeffX = mapConvert_resolutionX / mapConvert_fXToZ;
//            mapConvert_fCoeffY = mapConvert_resolutionY / mapConvert_fYToZ;
//        }
//
//        public static Vector3D MapDepthPercentPointToWorldSpace(Vector3D depthPercentPoint)
//        {
//            float arg_2F_0 = depthPercentPoint.X / mapConvert_resolutionX - 0.5f;
//            float num = 0.5f - depthPercentPoint.Y / mapConvert_resolutionY;
//            return new Vector3D(arg_2F_0 * depthPercentPoint.Z * mapConvert_fXToZ, num * depthPercentPoint.Z * mapConvert_fYToZ, depthPercentPoint.Z);
//        }
//
//        public static Vector3D MapWorldPointToDepthPercentSpace(Vector3D worldPoint)
//        {
//            return new Vector3D(mapConvert_fCoeffX * worldPoint.X / worldPoint.Z + mapConvert_nHalfXres, mapConvert_fCoeffY * worldPoint.Y / worldPoint.Z + mapConvert_nHalfYres, worldPoint.Z);
//        }
//
//        public static Vector3D MapDepthPointToWorldSpace(Vector3D depthPoint, Vector2D depthResolution)
//        {
//            Vector3D depthPercentPoint = new Vector3D (depthPoint.X / depthResolution.X, depthPoint.Y / depthResolution.Y, depthPoint.Z);
//            return MapDepthPercentPointToWorldSpace (depthPercentPoint);
//        }
//
//        public static Vector3D MapWorldPointToDepthSpace(Vector3D worldPoint, Vector2D depthResolution)
//        {
//            Vector3D depthPercentPoint = MapWorldPointToDepthPercentSpace (worldPoint);
//            return new Vector3D(depthPercentPoint.X * depthResolution.X, depthPercentPoint.Y * depthResolution.Y, depthPercentPoint.Z);
//        }
    }
}