using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float fireImpulse = 20f;
	public GameObject collisionEffect;

	// Use this for initialization
	void Awake () {
		if (collisionEffect != null) {
			GetComponent<Destructible> ().onCollision.AddListener (() => {
				Instantiate (collisionEffect, transform.position, collisionEffect.transform.rotation);
			});
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Fire (Vector2 direction) {
		float z = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;

		Vector3 angle = Vector3.zero;
		angle.z = z;
		transform.localRotation = Quaternion.Euler (angle);
		transform.GetComponent<Rigidbody2D> ().AddForce (direction * fireImpulse, ForceMode2D.Impulse);
	}
}
