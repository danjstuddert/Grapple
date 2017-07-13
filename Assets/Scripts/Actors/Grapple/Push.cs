using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DistanceJointController))]
public class Push : MonoBehaviour {
	public LayerMask pushableMask;
	public float pushForce;
	public float pushDistance;
	public float maxVelocity;

	public bool HasTarget { get; private set; }

	private Rigidbody2D rigidBody;

	private bool forceToApply;
	private Vector2 forceAmount;

	private Player player;
	private DistanceJointController jointController;

	public void Init(Player player, DistanceJointController jointController) {
		this.player = player;
		this.jointController = jointController;
		rigidBody = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		ApplyForce();
	}

	public void StartPush() {
		Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		target.z = 0f;

		RaycastHit2D hit = Physics2D.Raycast(transform.position, target - transform.position, pushDistance, pushableMask);

		if (hit) {
			HasTarget = true;
			jointController.ActivateJoint(pushDistance, hit);
		}
		else
			HasTarget = false;
	}

	public void UpdatePush() {
		if (HasTarget) {
			//Are we still in range of the point we are pushing away from?
			if (Vector2.Distance(transform.position, jointController.GetGrapplePoint()) < pushDistance) {
				//Get the direction of the player from the grapple point
				Vector3 direction = transform.position - jointController.GetGrapplePoint();

				//Apply force to the object in the direction
				forceToApply = true;
				forceAmount = direction * pushForce * Time.deltaTime;

				jointController.UpdateJoint(pushForce * Time.deltaTime);
			}
			else {
				player.DeactivateLineRenderer();
				EndPush();
			}
		}
	}

	public void EndPush() {
		jointController.DisableJoint();
		HasTarget = false;
	}

	private void ApplyForce() {
		if (forceToApply == false)
			return;

		forceToApply = false;
		rigidBody.AddForce(forceAmount, ForceMode2D.Impulse);

		//Make sure we aren't going too fast
		rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity, maxVelocity);
	}
}
