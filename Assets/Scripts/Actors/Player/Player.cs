using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType { Down, Hold, Release }

[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour {
	public float gravity = -20;

	private Controller2D controller;
	private Grab grab;
	private PlayerInput input;

	private Vector2 velocity;

	public void Init () {
		controller = GetComponent<Controller2D>();
		controller.Init();

		grab = GetComponent<Grab>();
		grab.Init();

		input = GetComponent<PlayerInput>();
		input.Init();
	}

	void Update() {
		UpdateController();
	}

	public void GrabInput(InputType type) {
		switch (type) {
			case InputType.Down:
				grab.StartGrab();
				break;
			case InputType.Hold:
				grab.UpdateGrab();
				break;
			case InputType.Release:
				grab.EndGrab();
				break;
		}
	}

	public void PushInput(InputType type) {

	}

	private void UpdateController() {
		if (controller == null)
			return;

		if (controller.collisions.above || controller.collisions.below)
			velocity.y = 0;
	}
}
