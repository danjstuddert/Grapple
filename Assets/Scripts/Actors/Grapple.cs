using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Grapple : MonoBehaviour {
	public float grappleSpeed;

	private Transform grapple;
	private LineRenderer line;

	private bool isGrappling;
	private bool isReturning;
	private Vector3 grappleTarget;

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
		UpdateGrapple();
		UpdateLineRenderer();
	}

	public void FireGrapple() {
		if (isGrappling)
			return;

		ActivateGrapple();
	}

	private void UpdateGrapple() {
		if (isGrappling == false)
			return;

		//if the grapple is returning make it come back to the player
		//otherwise go to the current grapple target
		GrappleToTarget(isReturning ? transform.position : grappleTarget);

		if (isReturning == false) {
			//if the grapple is at the grapple target make it come back
			if (grapple.position == grappleTarget)
				isReturning = true;
		}
		//If the grapple is at our position while returning stop grappling
		else if(grapple.position == transform.position) {
			DeactivateGrapple();
		}
	}

	private void GrappleToTarget(Vector3 target) {
		if (grapple.position != target)
			grapple.position = Vector2.MoveTowards(grapple.position, target, grappleSpeed * Time.deltaTime);
	}

	private void UpdateLineRenderer() {
		if (grapple == null || grapple.gameObject.activeInHierarchy == false)
			return;

		line.SetPosition(0, transform.position);
		line.SetPosition(1, grapple.position);
	}

	private void ActivateGrapple() {
		if (grapple.position != transform.position)
			grapple.position = transform.position;

		grapple.gameObject.SetActive(true);
		isGrappling = true;

		grappleTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		grappleTarget.z = transform.position.z;
		FaceGrappleTarget();
	}

	private void DeactivateGrapple() {
		isReturning = false;
		isGrappling = false;

		grapple.position = transform.position;
		grapple.gameObject.SetActive(false);
	}

	private void FaceGrappleTarget() {
		Quaternion rotation = Helpers.Instance.FaceObject(grapple.position, grappleTarget);
		grapple.rotation = rotation;
	}
}
