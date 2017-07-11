using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController> {

	void Start () {
		GameObject.FindWithTag("Player").GetComponent<Player>().Init();
		Camera.main.GetComponent<CameraController>().Init();
	}
}
