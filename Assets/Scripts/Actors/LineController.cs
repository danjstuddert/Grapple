using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour {
	private LineRenderer line;

	public void Init() {
		line = GetComponent<LineRenderer>();
	}

	public void ActivateLine() {
		line.enabled = true;
	}

	public void UpdateLine(Vector3 pos0, Vector3 pos1) {
		if (line.enabled == false)
			ActivateLine();

		if (line.GetPosition(0) != pos0)
			line.SetPosition(0, pos0);

		if (line.GetPosition(1) != pos1)
			line.SetPosition(1, pos1);
	}

	public void DeactivateLine() {
		line.enabled = false;
	}
}
