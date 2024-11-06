using System;
using System.Collections;
using Map;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MapEditorThemeItem : MonoBehaviour
{
	public Theme theme;

	private GameObject thumbnail;
	private float rotate = 0f;

	public void InitThumbnail()
	{
		thumbnail = new GameObject("ItemThumbnail", typeof(MeshFilter), typeof(MeshRenderer));
		thumbnail.gameObject.layer = LayerMask.NameToLayer("UI");
		thumbnail.transform.SetParent(transform);
		thumbnail.transform.localPosition = new Vector3(0, 0, -50);
		thumbnail.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
		thumbnail.GetComponent<MeshFilter>().mesh = theme.mesh;
		thumbnail.GetComponent<MeshRenderer>().material = theme.material;
	}

	private void Update()
	{
		// rotate += 5f * Time.deltaTime;
		if (thumbnail != null)
			thumbnail.transform.Rotate(0f, 15f * Time.deltaTime, 0f);
	}
}
