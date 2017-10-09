using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour {

	public float minSpeed = 0.1f;
	public float maxSpeed = 0.12f;
	float speed;
	public List<Sprite> possibleSprites;
	public List<Sprite> possibleDeathSprites;

	public int pointValue = 10;

	public float bounceDelay = 100f;
	float bounceTime;
	public float bounceHeight = 0.1f;

	bool bouncing = false, dead = false;

	int selectedSprite;

	// Use this for initialization
	void Start () {
		bounceTime = Time.time * 1000;
		this.speed = Random.value * (maxSpeed - minSpeed) + minSpeed;
		if (possibleSprites.Count > 0) {
			selectedSprite = Random.Range (0, possibleSprites.Count);
			GetComponent<SpriteRenderer> ().sprite = possibleSprites [selectedSprite];
		}
	}

	public void Die () {
		this.speed = 0;
		this.dead = true;
		GetComponent<SpriteRenderer> ().sprite = possibleDeathSprites [selectedSprite * 2 + Random.Range (0, 1)];
		GameObject bird = GameObject.Find ("Bird");
		bird.GetComponent<BirdController> ().addPoints (this.pointValue);
	}

	// Update is called once per frame
	void Update () {
		transform.localPosition -= new Vector3(this.speed * Time.deltaTime, 0, 0);
		if (!dead) {
			if (Time.time * 1000 - bounceTime > bounceDelay && !bouncing) {
				transform.position += new Vector3 (0, bounceHeight, 0);
				bouncing = true;
				bounceTime = Time.time * 1000;
			} else if (bouncing && Time.time * 1000 - bounceTime > bounceDelay / 2) {
				transform.position -= new Vector3 (0, bounceHeight, 0);
				bouncing = false;
			}
		}
	}
}
