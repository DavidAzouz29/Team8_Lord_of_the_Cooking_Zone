
/// Author Fraser Brown
/// 20/6/2016
/// https://www.youtube.com/watch?v=jOOdJZS987Y
using UnityEngine;
using System.Collections;

public class GrabAndDrop : MonoBehaviour
{
	GameObject grabbedObject;
	float grabbedObjectSize;

	Vector3 previousGrabPosition;

	float chargeTime= 0; 

	// Use this for initialization
	void Start ()
	{
	
	}

	/// <summary>
	/// Gets the mouse hover object.
	/// </summary>
	/// <returns>A GameObject that is within our range.</returns>
	/// <param name="range">Range.</param>
	GameObject GetMouseHoverObject (float range)
	{
		Vector3 position = gameObject.transform.position;
		RaycastHit raycastHit;
		Vector3 target = position + Camera.main.transform.forward * range;
		if (Physics.Linecast (position, target, out raycastHit)) {
			return raycastHit.collider.gameObject;
		}
		return null;
	}

	void TryGrabObject (GameObject grabObject)
	{
		if (grabObject == null)
			return;
		grabbedObject = grabObject;
		grabbedObjectSize = grabObject.GetComponent<Renderer> ().bounds.size.magnitude;
		grabObject.GetComponent<Rigidbody> ().useGravity = false;
	}

	void DropObject (float pushForce)
	{
		if (grabbedObject == null)
			return;
		Rigidbody rb = grabbedObject.GetComponent<Rigidbody> ();
		if (rb != null) {
			Vector3 throwVectory = grabbedObject.transform.position - previousGrabPosition;
			float speed = throwVectory.magnitude / Time.deltaTime;
			Vector3 throwVelocity = speed * throwVectory.normalized;

			throwVelocity += Camera.main.transform.forward * pushForce;

			rb.velocity = throwVelocity;
			rb.useGravity = true;
		}
		grabbedObject = null;
	}
	// Update is called once per frame
	void Update ()
	{
		Debug.Log (GetMouseHoverObject (5));
		// if we right click...
		if (Input.GetMouseButtonDown (1)) {
			if (grabbedObject == null)
				TryGrabObject (GetMouseHoverObject (5));
			else
				DropObject (0);
		}
		//
		if (Input.GetMouseButton (0))
			chargeTime += Time.deltaTime;
		//
		if (Input.GetMouseButtonUp (0)) {
			float pushForce = chargeTime * 20;
			pushForce = Mathf.Clamp (pushForce, 1, 100);
			DropObject (pushForce);
			chargeTime = 0;
		}
		if (grabbedObject != null) {
			previousGrabPosition = grabbedObject.transform.position;
			Vector3 newPosition = gameObject.transform.position + Camera.main.transform.forward * grabbedObjectSize;
			grabbedObject.transform.position = newPosition;
		}
	}
}