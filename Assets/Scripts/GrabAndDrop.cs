
/// Author Fraser Brown
/// 20/6/2016
/// https://www.youtube.com/watch?v=jOOdJZS987Y
using UnityEngine;
using System.Collections;

public class GrabAndDrop : MonoBehaviour
{
    public GameObject character;
	public GameObject grabbedObject;
	float grabbedObjectSize;

	Vector3 previousGrabPosition;

    private WeaponScript weapon;

//	public float chargeTime= 0; 

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
        Debug.DrawLine(position, target);
		if (Physics.Linecast (position, target, out raycastHit)) {
			return raycastHit.collider.gameObject;
		}
		return null;
	}

	void TryGrabObject (GameObject grabObject)
	{
        if (grabObject == null)
        {
            return;
        }

       weapon = grabObject.GetComponent<WeaponScript>();

        if (weapon.grabbable == false)
        {
            return;
        }

        weapon.grabbable = false;
		grabbedObject = grabObject;

        Rigidbody rb = grabObject.GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.velocity = new Vector3(0, 0, 0);
        grabObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
        grabObject.transform.parent = this.transform;
        Vector3 offset;

        Bounds Boundsize = grabObject.GetComponent<MeshRenderer>().bounds;

        offset.x = 0;
        offset.y = Boundsize.size.y / 2;
        offset.z = Boundsize.size.z / 2;

        grabObject.transform.localPosition = offset;
	}

	void DropObject (float pushForce)
	{
        if (grabbedObject == null)
        {
            return;
        }

        grabbedObject.transform.parent = null;
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;

        weapon.grabbable = true;
        weapon = null;
		grabbedObject = null;
	}

	// Update is called once per frame
	void Update ()
	{
		// if we right click...
		if(Input.GetButtonDown("Fire2"))
		{
			// if we don't have an object
			if (grabbedObject == null)
			{
                GameObject temp = GetMouseHoverObject(5);
                if (temp != null && temp.tag == "Weapon")
                {
                    TryGrabObject(temp);
                }
			}
			else
				DropObject (0);
		}
        //Shoot/Throw
		if(Input.GetButtonDown("Fire1"))
        {
            if (grabbedObject == null)
            {
                return;
            }

            grabbedObject.transform.parent = null;
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();

            rb.velocity = character.transform.right * weapon.throwSpeed;

            rb.isKinematic = false;

            grabbedObject = null;
        }
	}
}