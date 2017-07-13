using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DistanceJointController))]
public class Pull : MonoBehaviour {
	public LayerMask pullableMask;
	public float pullSpeed;
	public float pullDistance;

	public bool HasTarget { get; private set; }

	private DistanceJointController jointController;
	private Vector3 target;

	public void Init(DistanceJointController jointController) {
		this.jointController = jointController;
	}

	public void StartPull() {
		Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		target.z = 0f;

		RaycastHit2D hit = Physics2D.Raycast(transform.position, target - transform.position, pullDistance, pullableMask);

		if (hit) {
			HasTarget = true;
			jointController.ActivateJoint(pullDistance, hit);
		}
		else
			HasTarget = false;
	}

	public void UpdatePull() {
		if(HasTarget)
			jointController.UpdateJoint(-pullSpeed * Time.deltaTime);
	}

	public void EndPull() {
		jointController.DisableJoint();
	}
}
