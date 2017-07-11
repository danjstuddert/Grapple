using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
	private Player player;

	public void Init () {
		player = GetComponent<Player>();
	}
	
	void Update () {
		UpdateInput();
	}

	private void UpdateInput() {
		CheckGrabInput();
		CheckPullInput();
	}

	private void CheckGrabInput() {
		if(Input.GetButtonDown("Grab") && Input.GetButtonDown("Push") == false && Input.GetButton("Push") == false)
			player.GrabInput(InputType.Down);
		else if (Input.GetButton("Grab") && Input.GetButtonDown("Push") == false && Input.GetButton("Push") == false)
			player.GrabInput(InputType.Hold);
		else if (Input.GetButtonUp("Grab") && Input.GetButtonDown("Push") == false && Input.GetButton("Push") == false)
			player.GrabInput(InputType.Release);
	}

	private void CheckPullInput() {
		if (Input.GetButtonDown("Push") && Input.GetButtonDown("Grab") == false && Input.GetButton("Grab") == false)
			player.PushInput(InputType.Down);
		else if (Input.GetButton("Push") && Input.GetButtonDown("Grab") == false && Input.GetButton("Grab") == false)
			player.PushInput(InputType.Hold);
		else if (Input.GetButtonUp("Push") && Input.GetButtonDown("Grab") == false && Input.GetButton("Grab") == false)
			player.PushInput(InputType.Release);
	}
}
