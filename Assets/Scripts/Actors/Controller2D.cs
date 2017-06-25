using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {
	[Header("Ray Casting")]
	public float skinWidth;
	[Range(2, 20)]
	public int horizontalRayCount;
	[Range(2, 20)]
	public int verticalRayCount;
	public LayerMask collisionMask;

	public CollisionInfo collisions;

	private float horizontalRaySpacing;
	private float verticalRaySpacing;

	private BoxCollider2D myCollider;
	private RaycastOrigins raycastOrigins;

	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;

		public void Reset() {
			above = below = false;
			left = right = false;
		}
	}

	private struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	public void Init () {
		myCollider = GetComponent<BoxCollider2D>();
		CalculateRaySpacing();
	}

	public void Move(Vector2 velocity) {
		UpdateRaycastOrigins();
		collisions.Reset();

		if(velocity.x != 0)
			velocity = HorizontalCollisions(velocity);

		if(velocity.y != 0)
			velocity = VerticalCollisions(velocity);

		transform.Translate(velocity);
	}

	private Vector3 HorizontalCollisions(Vector3 velocity) {
		float directionX = Mathf.Sign(velocity.x);
		float rayLength = Mathf.Abs(velocity.x) + skinWidth;

		for (int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigin = directionX == -1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
			if (hit) {
				velocity.x = (hit.distance - skinWidth) * directionX;
				rayLength = hit.distance;

				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}
		}

		return velocity;
	}

	private Vector3 VerticalCollisions(Vector3 velocity) {
		float directionY = Mathf.Sign(velocity.y);
		float rayLength = Mathf.Abs(velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = directionY == -1 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
			if (hit) {
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}

		return velocity;
	}



	private void CalculateRaySpacing() {
		Bounds bounds = myCollider.bounds;
		bounds.Expand(skinWidth * -2f);

		horizontalRaySpacing = bounds.size.y / horizontalRayCount - 1;
		verticalRaySpacing = bounds.size.x / verticalRayCount - 1;
	}

	private void UpdateRaycastOrigins() {
		Bounds bounds = myCollider.bounds;
		bounds.Expand(skinWidth * -2f);

		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	private void OnValidate() {
		if (horizontalRayCount < 2)
			horizontalRayCount = 2;

		if (verticalRayCount < 2)
			verticalRayCount = 2;
	}
}
