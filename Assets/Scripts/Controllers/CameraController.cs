using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public Vector2 focusAreaSize;
	public float verticalOffset;
	public float lookAheadDistanceX;
	public float lookSmoothTimeX;
	public float verticalSmoothTime;

	private Collider2D targetCollider;
	private Rigidbody2D targetRigidBody;
	private FocusArea focusArea;

	private float currentLookAheadX;
	private float targetLookAheadX;
	private float lookAheadDirX;
	private float smoothLookVelocityX;
	private float smoothVelocityY;

	private bool lookAheadStopped;

	struct FocusArea {
		public Vector2 centre;
		public Vector2 velocity;

		private float left, right;
		private float top, bottom;

		public FocusArea(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x / 2;
			right = targetBounds.center.x + size.x / 2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;

			centre = new Vector2((left + right) / 2, (top + bottom) / 2);
			velocity = Vector2.zero;
		}

		public void Update(Bounds targetBounds) {
			float shiftX = 0;
			if (targetBounds.min.x < left) {
				shiftX = targetBounds.min.x - left;
			} else if(targetBounds.max.x > right) {
				shiftX = targetBounds.max.x - right;
			}
			left += shiftX;
			right += shiftX;

			float shiftY = 0;
			if (targetBounds.min.y < bottom) {
				shiftY = targetBounds.min.y - bottom;
			}
			else if (targetBounds.max.y > top) {
				shiftY = targetBounds.max.y - top;
			}
			top += shiftY;
			bottom += shiftY;

			centre = new Vector2((left + right) / 2, (top + bottom) / 2);
			velocity = new Vector2(shiftX, shiftY);
		}
	}

	public void Init() {
		targetCollider = GameObject.FindWithTag("Player").GetComponent<Collider2D>();
		targetRigidBody = targetCollider.GetComponent<Rigidbody2D>();
		focusArea = new FocusArea(targetCollider.GetComponent<Collider2D>().bounds, focusAreaSize);
	}

	void LateUpdate() {
		focusArea.Update(targetCollider.bounds);

		Vector2 focusPosition = focusArea.centre + Vector2.up * verticalOffset;

		if(focusArea.velocity.x != 0) {
			lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
			if(Mathf.Sign(targetRigidBody.velocity.x) == Mathf.Sign(focusArea.velocity.x) && targetRigidBody.velocity.x != 0) {
				lookAheadStopped = false;
				targetLookAheadX = lookAheadDirX * lookAheadDistanceX;
			}
			else {
				if(lookAheadStopped == false) {
					lookAheadStopped = true;
					targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDistanceX - currentLookAheadX) / 4f;
				}
			}
		}

		currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

		focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
		focusPosition += Vector2.right * currentLookAheadX;

		transform.position = (Vector3)focusPosition + Vector3.forward * -10;
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
		Gizmos.DrawCube(focusArea.centre, focusAreaSize);
	}
}
