using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DistanceJoint2D))]
public class DistanceJointController : MonoBehaviour {
	public float minJointLength;

	private DistanceJoint2D joint;
	private Vector3 grapplePoint;

	public void Init() {
		if (joint)
			return;

		joint = GetComponent<DistanceJoint2D>();
		DisableJoint();
	}
	
	public void ActivateJoint(float rayDistance, RaycastHit2D hitTransform) {
		if (hitTransform.collider != null && hitTransform.transform.GetComponent<Rigidbody2D>() != null) {
			joint.enabled = true;

			grapplePoint = hitTransform.point;
			Vector2 connectionPoint = hitTransform.point - (Vector2)hitTransform.transform.position;
			connectionPoint.x /= hitTransform.transform.localScale.x;
			connectionPoint.y /= hitTransform.transform.localScale.y;

			joint.connectedAnchor = connectionPoint;
			joint.connectedBody = hitTransform.transform.GetComponent<Rigidbody2D>();
			joint.distance = Vector2.Distance(transform.position, hitTransform.point);
		}
	}

	public void UpdateJoint(float adjustmentAmount) {
		if (joint.distance > minJointLength)
			joint.distance += adjustmentAmount;

		if (joint.distance < minJointLength)
			joint.distance = minJointLength;
	}

	public void DisableJoint() {
		joint.enabled = false;
	}

	public Vector3 GetGrapplePoint() {
		return grapplePoint;
	}
}
