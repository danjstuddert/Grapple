using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour {
	private Pull pull;
	private Push push;
	private PlayerInput input;
	private LineController lineRenderer;
	private DistanceJointController jointController;
	private Vector2 velocity;

	public void Init () {
		jointController = GetComponent<DistanceJointController>();
		jointController.Init();

		pull = GetComponent<Pull>();
		pull.Init(jointController);

		push = GetComponent<Push>();
		push.Init(this, jointController);

		input = GetComponent<PlayerInput>();
		input.Init();

		lineRenderer = GetComponent<LineController>();
		lineRenderer.Init();
	}

	public void PullInput(InputType type) {
		switch (type) {
			case InputType.Down:
				pull.StartPull();

				if (pull.HasTarget) {
					lineRenderer.ActivateLine();
					lineRenderer.UpdateLine(transform.position, jointController.GetGrapplePoint());
				}

				break;
			case InputType.Hold:
				pull.UpdatePull();

				if (pull.HasTarget)
					lineRenderer.UpdateLine(transform.position, jointController.GetGrapplePoint());
				else
					lineRenderer.DeactivateLine();

				break;
			case InputType.Release:
				pull.EndPull();
				lineRenderer.DeactivateLine();
				break;
		}
	}

	public void PushInput(InputType type) {
		switch (type) {
			case InputType.Down:
				push.StartPush();

				if (push.HasTarget) {
					lineRenderer.ActivateLine();
					lineRenderer.UpdateLine(transform.position, jointController.GetGrapplePoint());
				}

				break;
			case InputType.Hold:
				push.UpdatePush();

				if (push.HasTarget)
					lineRenderer.UpdateLine(transform.position, jointController.GetGrapplePoint());
				else
					lineRenderer.DeactivateLine();

				break;
			case InputType.Release:
				push.EndPush();
				lineRenderer.DeactivateLine();
				break;
		}
	}

	public void DeactivateLineRenderer() {
		lineRenderer.DeactivateLine();
	}
}
