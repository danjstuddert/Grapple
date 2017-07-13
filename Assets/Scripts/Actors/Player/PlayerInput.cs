using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType { Down, Hold, Release }

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
		if(Input.GetButtonDown("Pull") && Input.GetButtonDown("Push") == false && Input.GetButton("Push") == false)
			player.PullInput(InputType.Down);
		else if (Input.GetButton("Pull") && Input.GetButtonDown("Push") == false && Input.GetButton("Push") == false)
			player.PullInput(InputType.Hold);
		else if (Input.GetButtonUp("Pull") && Input.GetButtonDown("Push") == false && Input.GetButton("Push") == false)
			player.PullInput(InputType.Release);
	}

	private void CheckPullInput() {
		if (Input.GetButtonDown("Push") && Input.GetButtonDown("Pull") == false && Input.GetButton("Pull") == false)
			player.PushInput(InputType.Down);
		else if (Input.GetButton("Push") && Input.GetButtonDown("Pull") == false && Input.GetButton("Pull") == false)
			player.PushInput(InputType.Hold);
		else if (Input.GetButtonUp("Push") && Input.GetButtonDown("Pull") == false && Input.GetButton("Pull") == false)
			player.PushInput(InputType.Release);
	}
}
