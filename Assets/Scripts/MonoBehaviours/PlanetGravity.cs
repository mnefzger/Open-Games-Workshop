﻿using UnityEngine;
using System.Collections;

public class PlanetGravity : MonoBehaviour {

	public float gravity = -15f;
	
	void OnTriggerStay(Collider other)
    {
        Vector3 direction = (other.transform.position - transform.position).normalized;
        other.attachedRigidbody.AddForce(direction * gravity);
    }
}
