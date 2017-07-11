using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour {
	public LayerMask grabableMask;
	public float grabSpeed;
	public float grabDistance;

	private DistanceJoint2D joint;
	private LineRenderer lineRenderer;
	private Vector3 target;
	private Vector2 grapplePoint;

	public void Init() {
		joint = GetComponent<DistanceJoint2D>();
		joint.enabled = false;

		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;
	}

	public void StartGrab() {
		target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		target.z = 0f;

		RaycastHit2D hit = Physics2D.Raycast(transform.position, target - transform.position, grabDistance, grabableMask);

		if (hit.collider != null && hit.transform.GetComponent<Rigidbody2D>() != null) {
			joint.enabled = true;

			grapplePoint = hit.point;
			Vector2 connectionPoint = hit.point - (Vector2)hit.transform.position;
			connectionPoint.x /= hit.transform.localScale.x;
			connectionPoint.y /= hit.transform.localScale.y;

			joint.connectedAnchor = connectionPoint;
			joint.connectedBody = hit.transform.GetComponent<Rigidbody2D>();
			joint.distance = Vector2.Distance(transform.position, hit.point);

			lineRenderer.enabled = true;
			UpdateLineRenderer();
		}
	}

	public void UpdateGrab() {
		if (joint.distance > 0.5f)
			joint.distance -= grabSpeed * Time.deltaTime;

		UpdateLineRenderer();
	}

	public void EndGrab() {
		joint.enabled = false;
		lineRenderer.enabled = false;
	}

	private void UpdateLineRenderer() {
		if(lineRenderer.enabled) {
			lineRenderer.SetPosition(0, transform.position);

			if (lineRenderer.GetPosition(1) != (Vector3)grapplePoint)
				lineRenderer.SetPosition(1, grapplePoint);
		}
	}
}
