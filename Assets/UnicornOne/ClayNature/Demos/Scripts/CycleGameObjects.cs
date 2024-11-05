using System.Collections.Generic;
using UnityEngine;

namespace ClassNature {
	public class CycleGameObjects : MonoBehaviour {
		[SerializeField] float cycleTime;
		[SerializeField] float rotationSpeed;
		private float currentCycleTime;
		[SerializeField] float reduceTimePerCycle;
		[SerializeField] float minTimePerCycle;
		[SerializeField] List<GameObject> cycleList;

		private int currentCycle;
		private GameObject currentAsset;

		private void Start() {
			NewCycle();
		}

		void Update() {
			transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

			currentCycleTime += Time.deltaTime;
			if (currentCycleTime >= cycleTime) {
				UpdateCycle();
			}
		}

		private void UpdateCycle() {
			if (cycleList.Count >= 1) {
				currentCycleTime = 0f;
				cycleTime = Mathf.Clamp(cycleTime * reduceTimePerCycle, minTimePerCycle, cycleTime);
				cycleList.RemoveAt(currentCycle);
			} else {
				return;
			}
			Destroy(currentAsset);
			NewCycle();
		}

		private void NewCycle() {
			currentCycle = Random.Range(0, cycleList.Count);
			currentAsset = Instantiate(cycleList[currentCycle], gameObject.transform);
		}
	}
}
