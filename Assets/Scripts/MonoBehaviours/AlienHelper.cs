﻿using UnityEngine;
using System.Collections;

public class AlienHelper : CreatureHelper {

	Alien alien;
    GameState gs;
    bool movingToShipWithResource = false;
	bool infectionReady = true;

	public override void Start () {
		base.Start ();
        gs = GameObject.Find("GameState").GetComponent<GameState>();
        alien = gs.aliens[gameObject] as Alien;
        gameObject.GetComponent<SphereCollider>().radius = alien.VisionRange;
    }

	public override void Update () {
        base.Update ();

        if (alien == null)
		{
			alien = gs.aliens [gameObject] as Alien;
		} 
		else if (alien.state == Alien.AlienState.SEARCHING && !alien.movingToResource)
		{
			alien.Search ();
		} 
		else if (alien.state == Alien.AlienState.CARRYING)
		{
			alien.CarryResource ();
		} 
		else if (alien.state == Alien.AlienState.FLEEING)
		{
			alien.Flee();
		}

        if (alien.movingToResource) CheckDistToResource();
        if (movingToShipWithResource) CheckDistToShip();
		if (alien.Infected && infectionReady)
		{
			infectionReady = false;
			StartCoroutine(InfectionDamage(3));
		}
    }

    public override void NoPathFound()
    {
        base.NoPathFound();
        alien.ResetTarget();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "resource" && alien.state == Alien.AlienState.SEARCHING)
        {
            // check if resource is free to pick up
            if(DoesNotBelongToOtherAlien(other.gameObject))
            {
                Debug.Log("Detected Resource");
                alien.MoveTo(other.gameObject.transform.position);
                alien.movingToResource = true;
                alien.Resource = other.gameObject;
            }
            
        }
    }

    void OnTriggerLeave(Collider other)
    {
        if (other.gameObject.name == "resource")
        {
            alien.Resource = null;
        }
    }

    void CheckDistToResource()
    {
        float dist = (gameObject.transform.position - alien.Resource.transform.position).magnitude;
        if (dist <= 2.5f)
        {
            alien.TakeResource();
            alien.movingToResource = false;
            movingToShipWithResource = true;
        }
    }

    void CheckDistToShip()
    {
        float dist = (gameObject.transform.position - GameValues.ShipPos).magnitude;
        if (dist <= 3f)
        {        
            gs.CollectedResources += 1;
            GameObject res = alien.Resource;
            // check if other Aliens were trying to reach this specific resource too
            RemoveResourceReferences(res);
            Destroy(res);
            movingToShipWithResource = false;
            Debug.Log("Deposit Resource. Collected Resources: " + gs.CollectedResources);
        }
    }

    bool DoesNotBelongToOtherAlien(GameObject res)
    {
        foreach (DictionaryEntry d in gs.aliens)
        {
            Alien a = d.Value as Alien;
            if (a.Resource == res)
            {
                return false;
            }
        }
        return true;
    }

    void RemoveResourceReferences(GameObject res)
    {
        foreach(DictionaryEntry d in gs.aliens)
        {
            Alien a = d.Value as Alien;
            if (a.Resource == res)
            {
                a.DropResource();
                a.state = Alien.AlienState.SEARCHING;
            }
        }
    }

	IEnumerator InfectionDamage(float sec)
	{
		alien.TakeDamage (2);
		yield return new WaitForSeconds (sec);
		infectionReady = true;
	}
}
