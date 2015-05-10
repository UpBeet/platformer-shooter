using UnityEngine;
using System.Collections;

public class Enemy : Destructible {

	// Use this for initialization
	void Awake () {
		onDealDamage += (amount) => {
			GetComponent<Animator> ().Play ("SnakeAlienHurt", -1, 0);
		};
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
