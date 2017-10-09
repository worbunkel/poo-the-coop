using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopController : MonoBehaviour {

	float yVelocity = 0.5f;
	float yAcceleration = 1f;
	float maxYVelocity = 1.5f;

	bool dead;

	public List<Sprite> animationSprites;
	public float animationDelay;
	public Sprite deadSprite;

	AnimationController anim;

	void Start () {
		anim = new AnimationController (gameObject, this.animationSprites, this.animationDelay);
	}

	void Update () {
		if (!this.dead) {
			this.yVelocity = Mathf.Min (this.yAcceleration * Time.deltaTime + this.yVelocity, maxYVelocity);
			transform.position -= new Vector3 (0, this.yVelocity * Time.deltaTime, 0);
			anim.update ();
		} else {
			GetComponent<SpriteRenderer> ().sprite = deadSprite;
		}
	}

	private void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "HurtOnTouch") {
			this.dead = true;
			CarController carController = coll.gameObject.GetComponent<CarController> ();
			CatController catController = coll.gameObject.GetComponent<CatController> ();
			HumanController humanController = coll.gameObject.GetComponent<HumanController> ();
			if (carController != null) {
				carController.Die ();
				coll.gameObject.tag = "Ground";
			} else if (catController != null) {
				catController.Die ();
			} else if (humanController != null) {
				humanController.Die ();
			}
			transform.SetParent (coll.transform);
		} else if (coll.gameObject.tag == "Ground") {
			this.dead = true;
			transform.SetParent (coll.transform);
		}
		GetComponent<Collider2D> ().enabled = false;
	}
}
