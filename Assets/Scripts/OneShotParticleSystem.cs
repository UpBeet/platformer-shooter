using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ParticleSystem))]
public class OneShotParticleSystem : MonoBehaviour {

	ParticleSystem system { get { return this.GetComponent<ParticleSystem> (); } }
	
	// Update is called once per frame
	void Update () {
		if (!system.IsAlive ()) {
			Destroy (gameObject);
		}
	}
}
