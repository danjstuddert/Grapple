using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour {
	public bool HasCollided { get; private set; }
	public Vector2 CollisionPosition { get; private set; }

	public void DeactivateGrapple() {
		HasCollided = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.transform != transform.parent) {
			HasCollided = true;
			CollisionPosition = transform.position;
		}		
	}
}
