using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Destructible : MonoBehaviour {

	[Header ("Life Span")]
	public bool destroyOnAgeOut = false;
	public float lifeSpan = 10f;
	private float lifetime = 0f;

	[Header ("Health")]
	public bool destroyOnZeroHealth = false;
	public bool invulnerable = false;
	public float maxHealth;
	public float health = 100f;

	[Header ("Collisions")]
	public bool destroyOnCollision = false;

	[Header ("Events")]
	public UnityEvent onAgedOut, onZeroHealth, onCollision;
	public UnityEvent<float> onHeal, onDealDamage;

	// Use this for initialization
	void Start () {
		lifetime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateLifeSpan ();
	}

	void OnCollisionEnter2D () {
		onCollision.Invoke ();
		if (destroyOnCollision) {
			GameObject.Destroy (this.gameObject);
		}
	}

	private void DealDamage (float damageAmount) {
		if (!invulnerable) {
			onDealDamage.Invoke (damageAmount);
			health -= damageAmount;
			if (health <= 0) {
				health = 0;
				onZeroHealth.Invoke ();
				if (destroyOnZeroHealth) {
					GameObject.Destroy (this.gameObject);
				}
			}
		}
	}

	private void Heal (float healAmount) {
		onHeal.Invoke (healAmount);
		health += healAmount;
		if (health > maxHealth) { health = maxHealth; }
	}

	private void UpdateLifeSpan () {
		lifetime += Time.deltaTime;
		if (lifetime >= lifeSpan) {
			if (onAgedOut != null) onAgedOut.Invoke ();
			if (destroyOnAgeOut) {
				GameObject.Destroy (this.gameObject);
			}
		}
	}
}