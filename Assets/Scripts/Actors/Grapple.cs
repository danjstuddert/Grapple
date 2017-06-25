using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Grapple : MonoBehaviour {
	public float grappleSpeed;

	private Transform grapple;
	private LineRenderer line;

	public void Init() {
		for (int i = 0; i < transform.childCount; i++) {
			if(transform.GetChild(i).name == "Grapple") {
				grapple = transform.GetChild(i);
				break;
			}
		}

		if (grapple == null)
			Debug.LogError(string.Format("{0} does not contain a grapple but has the grapple script attached!", name));

		line = grapple.GetComponent<LineRenderer>();

		DeactivateGrapple();
	}

	void Update() {
		UpdateLineRenderer();
	}

	public void FireGrapple() {
		Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	private void UpdateLineRenderer() {
		if (grapple == null || grapple.gameObject.activeInHierarchy == false)
			return;

		line.SetPosition(0, transform.position);
		line.SetPosition(1, grapple.position);
	}

	private void DeactivateGrapple() {
		grapple.position = transform.position;
		grapple.gameObject.SetActive(false);
	}
}
