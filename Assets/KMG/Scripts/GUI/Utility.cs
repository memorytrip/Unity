using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GUI
{
    public static class Utility 
    {
        public static bool IsPointOverGUI()
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.layer == LayerMask.NameToLayer("UI"))
                    return true;
            }

            return false;
            // reference : https://trialdeveloper.tistory.com/139 [오니부기 개발로그:티스토리]
        }
    }

}
