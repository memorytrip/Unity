using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GUI
{
    public static class Utility 
    {
        
        public static bool IsPointOverGUI()
        {
            return IsPointOverGUI(Input.mousePosition);
        }
        
        public static bool IsPointOverGUI(Vector2 point)
        {
            List<RaycastResult> results = RaycastWithPoint(point);

            for (int i = 0; i < results.Count; i++) {
                if (results[i].gameObject.layer == LayerMask.NameToLayer("UI"))
                    return true;
            }

            return false;
			// reference : https://trialdeveloper.tistory.com/139 [오니부기 개발로그:티스토리]
			// reference : https://bonnate.tistory.com/135 - inputaction callback에선 작동하지 않는듯 하다
		}

		public static List<RaycastResult> RaycastWithPoint(Vector2 point) {
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = point;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);
            return results;
        }
    }

}
