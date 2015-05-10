using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	[Header ("Movement")]
	public float moveSpeed = 1f;
	public float jumpForce = 1f;
	
	private bool grounded = false;

	[Header ("Shooting")]
	public Projectile bulletPrefab;
	[Range(0f,1f)]public float accuracy = 0.8f;
	public float stability = 1f;
	public float fireRate = 1f;
	public float shotKickback = 0.01f;

	private float kickback = 0f;
	private float fireCooldown = 0f;

	public bool FacingRight {
		get { return facingRight; }
		set {
			if (facingRight == value) return;
			Face (value);
		}
	}
	private bool facingRight = true;

	private bool CanFire {
		get {
			return fireCooldown <= 0;
		}
	}

	private Transform GroundCheck {
		get {
			if (groundCheck == null) {
				groundCheck = transform.FindChild ("Ground Check");
			}
			return groundCheck;
		}
	}
	private Transform groundCheck;

	private Image AimCone {
		get {
			if (aimCone == null) {
				aimCone = transform.FindChild ("Aim Cone").GetComponent<Image> ();
			}
			return aimCone;
		}
	}
	private Image aimCone;

	private float aimAngle = 0f;

	private Rigidbody2D rigidbody2D { get { return GetComponent<Rigidbody2D> (); } }
	private Animator animator { get { return GetComponent<Animator> (); } }

	// Use this for initialization
	void Start () {
		EquipGun ();
	}
	
	// Update is called once per frame
	void Update () {
		CheckGrounded ();
		HandleInput ();
		UpdateFireRate ();
		UpdateAim ();
	}

	private void CheckGrounded () {
		grounded = Physics2D.Linecast (transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
	}

	private void HandleInput () {
		float h = Input.GetAxis ("Horizontal");

		if (h < 0) FacingRight = false;
		else if (h > 0) FacingRight = true;

		transform.Translate (Vector3.right * h * moveSpeed * Time.deltaTime);

		if (grounded) {
			if (h != 0) {
				animator.Play ("AlienWalk");
				animator.speed = Mathf.Abs (h);
			}
			else animator.Play ("AlienStand");
		}

		if (Input.GetButtonDown ("Jump")) { Jump (); }
		if (Input.GetButton ("Fire1")) { Fire (); }

		if (Input.GetKey (KeyCode.UpArrow)) { aimAngle += Time.deltaTime * 10; }
		if (Input.GetKey (KeyCode.DownArrow)) { aimAngle -= Time.deltaTime * 10; }
	}

	private void UpdateFireRate () {
		if (fireCooldown >= 0) {
			fireCooldown -= fireRate * Time.deltaTime;
		}
		else {
			fireCooldown = 0;
		}
	}

	private void UpdateAim () {
		AimWeapon (aimAngle);

		if (kickback > 0) {
			kickback -= stability * Time.deltaTime;
		}
		else kickback = 0;

		float actualAccuracy = accuracy - kickback;
		float fill = 1 - actualAccuracy;
		AimCone.fillAmount = fill;

		Vector3 angle = Vector3.zero;
		angle.z = (360f * fill) / 2f;
		AimCone.transform.localRotation = Quaternion.Euler (angle);
	}

	private void Jump () {
		if (grounded) {
			rigidbody2D.AddForce (Vector2.up * jumpForce, ForceMode2D.Impulse);
			animator.Play ("AlienJump");
		}
	}

	private void Fire () {
		if (CanFire) {
			Projectile projectile = (Projectile)Instantiate (bulletPrefab, AimCone.transform.position, Quaternion.identity);
			projectile.Fire (GetRandomFireAngle () * transform.localScale.x);
			fireCooldown = 1;
			kickback += shotKickback;

			transform.FindChild ("Weapon").FindChild ("Fire Point").GetComponent<FlashController> ().Flash ();
		}
	}

	private void Face (bool facingRight) {
		Vector3 newScale = transform.localScale;
		newScale.x *= -1;
		transform.localScale = newScale;
		this.facingRight = facingRight;
	}

	private Vector2 GetRandomFireAngle () {
		float offsetRange = 360f * (1 - (accuracy - kickback)) / 2;
		float randOffset = Random.Range (-offsetRange, offsetRange);
		float radianAngle = (randOffset + AimCone.transform.rotation.z + aimAngle) * Mathf.Deg2Rad;

		return new Vector2((float)Mathf.Cos(radianAngle), (float)Mathf.Sin(radianAngle));
	}

	private void AimWeapon (float angle) {
		transform.FindChild ("Weapon").rotation = Quaternion.Euler (0, 0, angle);
	}

	private void EquipGun () {
		Transform firePoint = transform.FindChild ("Weapon").FindChild ("Fire Point");
		AimCone.transform.position = firePoint.position;
		AimCone.transform.SetParent (firePoint);
	}
}
