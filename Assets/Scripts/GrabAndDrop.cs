
/// Author Fraser Brown
/// 20/6/2016
/// https://www.youtube.com/watch?v=jOOdJZS987Y
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabAndDrop : MonoBehaviour
{
    public GameObject character;
	public GameObject grabbedObject;
    public GameObject hand;
	float grabbedObjectSize;

    public List<GameObject> pickups;

	Vector3 previousGrabPosition;

    private WeaponScript weapon;

//	public float chargeTime= 0; 

	// Use this for initialization
	void Start ()
	{
	
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

        grabObject.tag = "Held";
        rb.isKinematic = true;
        rb.velocity = new Vector3(0, 0, 0);
        grabObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
        grabObject.transform.parent = hand.transform;
        Vector3 offset;

        Bounds Boundsize = grabObject.GetComponent<MeshRenderer>().bounds;

        offset.x = 0;
        offset.y = Boundsize.size.y / 2;
        offset.z = Boundsize.size.z / 2;

        grabObject.transform.localPosition = offset;
        grabObject.GetComponent<Collider>().enabled = false;

        pickups.Remove(grabObject);
	}

//	void DropObject (float pushForce)
//	{
//        if (grabbedObject == null)
//        {
//            return;
//        }
//
//        grabbedObject.transform.parent = null;
//        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
//
//        weapon.grabbable = true;
//        weapon = null;
//		grabbedObject = null;
//	}

	// Update is called once per frame
	void Update ()
	{
		// if we right click...
		if(Input.GetButtonDown("Fire2"))
		{
            // if we don't have an object
            if (grabbedObject == null)
            {
                GameObject temp = GetNearestPickup();
                if (temp != null && temp.tag == "Pickup")
                {
                    TryGrabObject(temp);
                }
            }
//            else
//            {
//                DropObject(0);
//            }
		}
        //Shoot/Throw
		if(Input.GetButtonDown("Fire1"))
        {
            if (grabbedObject == null)
            {
                return;
            }

            ThrowObject();
        }
	}

    void ThrowObject()
    {
        grabbedObject.transform.parent = null;
        Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();

        rb.velocity = character.transform.right * weapon.throwSpeed;

        rb.isKinematic = false;

        Vector3 throwPos;

        throwPos = character.transform.position;
        throwPos.y += character.GetComponentInChildren<SkinnedMeshRenderer>().bounds.size.y;
        throwPos.x += character.GetComponent<CapsuleCollider>().radius;
        throwPos.x += grabbedObject.GetComponent<MeshRenderer>().bounds.size.x / 2;

        grabbedObject.transform.position = throwPos;

        grabbedObject.tag = "Weapon";
        grabbedObject.GetComponent<BoxCollider>().enabled = true;

        grabbedObject = null;
    }

    GameObject GetNearestPickup()
    {
        if (pickups.Count == 0)
        {
            return null;
        }
        
        //If object in array exists, assign as closest gameobject
        GameObject temp = pickups[0];

        float distance = (temp.transform.position - character.transform.position).magnitude;

        for (int i = 1; i < pickups.Count; ++i)
        {
            if ((pickups[i].transform.position - character.transform.position).magnitude < distance)
            {
                temp = pickups[i];
            }
        }

        return temp;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Pickup")
        {
            return;
        }

        pickups.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        pickups.Remove(other.gameObject);
    }
}