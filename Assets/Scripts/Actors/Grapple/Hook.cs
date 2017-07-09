using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {
	public LayerMask hookableMask;
	public float hookSpeed;

	private DistanceJoint2D joint;
	private LineRenderer lineRenderer;
	private Vector3 target;

	public void Init() {
		joint = GetComponent<DistanceJoint2D>();
		joint.enabled = false;

		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateHook();
	}

	private void UpdateHook() {
		if (joint.distance > 0.5f)
			joint.distance -= hookSpeed * Time.deltaTime;

		if (Input.GetMouseButtonDown(0)) {
			target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			target.z = 0f;

			RaycastHit2D hit = Physics2D.Raycast(transform.position, target - transform.position, Mathf.Infinity, hookableMask);

			if(hit.collider != null && hit.transform.GetComponent<Rigidbody2D>() != null) {
				joint.enabled = true;

				Vector2 connectionPoint = hit.point - (Vector2)hit.transform.position;
				connectionPoint.x /= hit.transform.localScale.x;
				connectionPoint.y /= hit.transform.localScale.y;

				joint.connectedAnchor = connectionPoint;
				joint.connectedBody = hit.transform.GetComponent<Rigidbody2D>();
				joint.distance = Vector2.Distance(transform.position, hit.point);

				lineRenderer.enabled = true;
				lineRenderer.SetPosition(0, transform.position);
				lineRenderer.SetPosition(1, hit.point);
			}
		}
		else if (Input.GetMouseButton(0)) {
			lineRenderer.SetPosition(0, transform.position);
		}

		else if (Input.GetMouseButtonUp(0)) {
			joint.enabled = false;
			lineRenderer.enabled = false;
		}
	}
}
