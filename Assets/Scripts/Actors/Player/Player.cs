using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(Grapple))]
public class Player : MonoBehaviour {
	public float gravity = -20;

	private Controller2D controller;
	private Grapple grapple;

	private Vector2 velocity;

	void Start () {
		controller = GetComponent<Controller2D>();
		controller.Init();

		grapple = GetComponent<Grapple>();
		grapple.Init();
	}

	void Update() {
		UpdateInput();
		UpdateController();
	}

	private void UpdateInput() {
		if (Input.GetMouseButtonDown(0)) {
			grapple.FireGrapple();
		}
	}

	private void UpdateController() {
		if (controller.collisions.above || controller.collisions.below)
			velocity.y = 0;

		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
}
