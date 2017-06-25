using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public Vector2 focusAreaSize;

	private Controller2D target;
	private FocusArea focusArea;
	
	public void Init () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	struct FocusArea {
		public Vector2 centre;
		public Vector2 velocity;
		float left, right;
		float top, bottom;

		public FocusArea(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x / 2;
			right = targetBounds.center.x + size.x / 2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;

			centre = new Vector2((left + right) / 2, (top + bottom) / 2);
			velocity = Vector2.zero;
		}

		public void Update(Bounds targetBounds) {
			float shiftX = 0f;
			if (targetBounds.min.x < left)
				shiftX = targetBounds.min.x - left;
			else if (targetBounds.max.x > right)
				shiftX = targetBounds.max.x - right;

			left += shiftX;
			right += shiftX;

			float shiftY = 0f;
			if (targetBounds.min.x < bottom)
				shiftY = targetBounds.min.y - bottom;
			else if (targetBounds.max.x > top)
				shiftY = targetBounds.max.y - top;

			top += shiftY;
			bottom += shiftY;

			centre = new Vector2((left + right) / 2, (top + bottom) / 2);
			velocity = new Vector2(shiftX, shiftY);
		}
	}
}
