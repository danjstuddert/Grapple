using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(GrappleController))]
public class Player : MonoBehaviour {
	public float gravity = -20;

	private Controller2D controller;
	private GrappleController grapple;
	private Hook hook;

	private Vector2 velocity;

	public void Init () {
		controller = GetComponent<Controller2D>();
		controller.Init();

		hook = GetComponent<Hook>();
		hook.Init();

		//grapple = GetComponent<GrappleController>();
		//grapple.Init();
	}

	void Update() {
		UpdateInput();
		UpdateController();
	}

	private void UpdateInput() {
		if (grapple == null)
			return;

		if (Input.GetMouseButtonDown(0)) {
			grapple.FireGrapple();
		}
	}

	private void UpdateController() {
		if (controller == null)
			return;

		if (controller.collisions.above || controller.collisions.below)
			velocity.y = 0;

		//if (grapple.IsGrappling) {
		//	velocity.y = 0f;
		//}
		//else {
		//	velocity.y += gravity * Time.deltaTime;
		//	controller.Move(velocity * Time.deltaTime);
		//}
	}
}
