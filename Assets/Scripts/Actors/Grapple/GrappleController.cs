using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrappleController : MonoBehaviour {
	public float grappleSpeed;

	private Transform grappleTransform;
	private Grapple grapple;
	private LineRenderer line;

	public bool IsGrappling { get; private set; }
	private bool isReturning;
	private Vector3 grappleTarget;

	public void Init() {
		for (int i = 0; i < transform.childCount; i++) {
			if(transform.GetChild(i).name == "Grapple") {
				grappleTransform = transform.GetChild(i);
				break;
			}
		}

		if (grappleTransform == null)
			Debug.LogError(string.Format("{0} does not contain a grapple but has the grapple script attached!", name));

		grapple = transform.GetChild(0).GetComponent<Grapple>();

		line = grappleTransform.GetComponent<LineRenderer>();

		DeactivateGrapple();
	}

	void Update() {
		UpdateGrapple();
		UpdateLineRenderer();
	}

	public void FireGrapple() {
		if (IsGrappling)
			return;

		ActivateGrapple();
	}

	private void UpdateGrapple() {
		if (IsGrappling == false)
			return;

		if (grapple.HasCollided) {
			MoveToGrapple();
		}
		else {
			//if the grapple is returning make it come back to the player
			//otherwise go to the current grapple target
			GrappleToTarget(isReturning ? transform.position : grappleTarget);

			if (isReturning == false) {
				//if the grapple is at the grapple target make it come back
				if (grappleTransform.position == grappleTarget)
					isReturning = true;
			}
			//If the grapple is at our position while returning stop grappling
			else if (grappleTransform.position == transform.position) {
				DeactivateGrapple();
			}
		}
	}

	private void GrappleToTarget(Vector3 target) {
		if ((Vector2)grappleTransform.position != grapple.CollisionPosition)
			grappleTransform.position = grapple.CollisionPosition;

		if (grappleTransform.position != target)
			grappleTransform.position = Vector2.MoveTowards(grappleTransform.position, target, grappleSpeed * Time.deltaTime);
	}

	private void MoveToGrapple() {
		if(transform.position != grappleTransform.position) {
			transform.position = Vector2.MoveTowards(transform.position, grappleTransform.position, grappleSpeed * Time.deltaTime);
		}
	}

	private void UpdateLineRenderer() {
		if (grappleTransform == null || grappleTransform.gameObject.activeInHierarchy == false)
			return;

		line.SetPosition(0, transform.position);
		line.SetPosition(1, grappleTransform.position);
	}

	private void ActivateGrapple() {
		if (grappleTransform.position != transform.position)
			grappleTransform.position = transform.position;

		grappleTransform.gameObject.SetActive(true);
		IsGrappling = true;

		grappleTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		grappleTarget.z = transform.position.z;
		FaceGrappleTarget();
	}

	private void DeactivateGrapple() {
		isReturning = false;
		IsGrappling = false;

		grappleTransform.position = transform.position;
		grappleTransform.gameObject.SetActive(false);
	}

	private void FaceGrappleTarget() {
		Quaternion rotation = Helpers.Instance.FaceObject(grappleTransform.position, grappleTarget);
		grappleTransform.rotation = rotation;
	}
}
