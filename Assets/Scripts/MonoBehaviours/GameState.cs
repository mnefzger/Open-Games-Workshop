﻿using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

    /* To be used like this
	 * CREATE
	 * ShyMonster m = new ShyMonster (1,1,0.15f,1);
	 * m.GameObject = Creator.Create ("monster", new Vector3(0,80,-100));
	 * gs.monsters.Add (m.GameObject, m);
	 * 
	 * USE
	 * Monster c = gs.monsters [transform.gameObject] as Monster;
	 * 
	*/

    public Hashtable creatures = new Hashtable();
	public Hashtable monsters = new Hashtable();
	public Hashtable aliens = new Hashtable();

	public float timeLeft;
	public int CollectedResources { get; set; }
    public int ActiveSkill { get; set; }

	void RemoveCreature(Creature c) {
		if (c is Monster) {
			monsters.Remove (c.GameObject);
			Debug.Log("Removed monster");
		} else {
			aliens.Remove (c.GameObject);
		}
        creatures.Remove(c.GameObject);
	}

}
