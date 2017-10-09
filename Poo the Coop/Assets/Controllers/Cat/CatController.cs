using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour {

	public List<Sprite> sittingAnimationSprites;
	public float sittingAnimationDelay = 500f;
	public Sprite crouchingSprite;
	public Sprite leapingSprite;

	public float jumpYVelocity = -1.5f;
	public float jumpXVelocity = -1f;
	public float jumpYAcceleration = 1.5f;
	public int pointValue = 20;
	float yVelocity = 0, xVelocity = 0, yAcceleration = 0;

	bool leaped;

	GameObject bird;

	AnimationController sittingAnim;

	void Start () {
		bird = GameObject.Find ("Bird");
		sittingAnim = new AnimationController (gameObject, sittingAnimationSprites, sittingAnimationDelay);
	}

	void Update () {
		if (bird == null) {
			bird = GameObject.Find ("Bird");
		}
		yVelocity +=  yAcceleration * Time.deltaTime;
		transform.position += new Vector3 (xVelocity * Time.deltaTime, -yVelocity * Time.deltaTime, 0);
		if (transform.position.x - bird.transform.position.x < 1.25f && !leaped) {
			GetComponent<SpriteRenderer> ().sprite = crouchingSprite;
		}
		if (transform.position.x - bird.transform.position.x < 1f && !leaped) {
			leap ();
		} else if (this.leaped) {
			GetComponent<SpriteRenderer> ().sprite = leapingSprite;
		}else {
			sittingAnim.update ();
		}
	}

	void leap(){
		xVelocity = jumpXVelocity;
		yVelocity = jumpYVelocity;
		yAcceleration = jumpYAcceleration;
		this.leaped = true;
		gameObject.transform.localScale = gameObject.transform.localScale * 1.5f;
	}

	public void Die(){
		xVelocity = 0;
		yVelocity = 1f;
		gameObject.transform.localScale = 2f * gameObject.transform.localScale / 3f;


		GameObject bird = GameObject.Find ("Bird");
		bird.GetComponent<BirdController> ().addPoints (this.pointValue);
	}
}
