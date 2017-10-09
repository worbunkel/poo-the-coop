using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

	public float minSpeed = 0.25f;
	public float maxSpeed = 0.5f;
	public int pointValue = 5;
	float speed;
	public List<Sprite> possibleSprites;

	// Use this for initialization
	void Start () {
		this.speed = Random.value * (maxSpeed - minSpeed) + minSpeed;
		if (possibleSprites.Count > 0) {
			GetComponent<SpriteRenderer> ().sprite = possibleSprites [Random.Range (0, possibleSprites.Count)];
		}
	}

	public void Die () {
		GameObject bird = GameObject.Find ("Bird");
		bird.GetComponent<BirdController> ().addPoints (this.pointValue);
		this.speed = 0;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition -= new Vector3(this.speed * Time.deltaTime, 0, 0);
	}
}
