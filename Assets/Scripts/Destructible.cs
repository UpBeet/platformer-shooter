using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public delegate void HealthChangeEventHandler (float amount);
public delegate void Collision2DEventHandler (Collision2D collision);
	
public class Destructible : MonoBehaviour {

	[Header ("Life Span")]
	public bool destroyOnAgeOut = false;
	public float lifeSpan = 10f;
	private float lifetime = 0f;

	[Header ("Health")]
	public bool destroyOnZeroHealth = false;
	public bool invulnerable = false;
	public float health = 100f;
	private float maxHealth;

	[Header ("Collisions")]
	public bool destroyOnCollision = false;

	[Header ("Events")]
	public UnityEvent onAgedOut, onZeroHealth;
	public event Collision2DEventHandler onCollision2D;
	public event HealthChangeEventHandler onHeal, onDealDamage;

	// Use this for initialization
	void Awake () {
		lifetime = 0f;
		maxHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateLifeSpan ();
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (onCollision2D != null) onCollision2D (collision);
		if (destroyOnCollision) {
			GameObject.Destroy (this.gameObject);
		}
	}

	public void DealDamage (float damageAmount) {
		if (!invulnerable) {
			if (onDealDamage != null) onDealDamage (damageAmount);
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

	public void Heal (float healAmount) {
		if (onHeal != null) onHeal (healAmount);
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