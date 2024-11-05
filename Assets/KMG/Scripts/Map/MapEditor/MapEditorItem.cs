using System;
using System.Collections;
using Map;
using UnityEngine;
using UnityEngine.UI;

public class MapEditorItem : MonoBehaviour
{
	public Model model;

	private GameObject thumbnail;
	private float rotate = 0f;

	public void InitThumbnail()
	{
		thumbnail = new GameObject("ItemThumbnail", typeof(MeshFilter), typeof(MeshRenderer));
		thumbnail.gameObject.layer = LayerMask.NameToLayer("UI");
		thumbnail.transform.SetParent(transform);
		thumbnail.transform.localPosition = new Vector3(0, -65, 0);
		thumbnail.GetComponent<MeshFilter>().mesh = model.mesh;
		thumbnail.GetComponent<MeshRenderer>().material = model.material;
	}

	private void Update()
	{
		// rotate += 5f * Time.deltaTime;
		if (thumbnail != null)
			thumbnail.transform.Rotate(0f, 15f * Time.deltaTime, 0f);
	}
}
