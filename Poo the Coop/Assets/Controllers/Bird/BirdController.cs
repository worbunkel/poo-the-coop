using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdController : MonoBehaviour {

	float x, y;
	float origY;
	float yAcceleration = 3f;
	float origYAcceleration;
	float maxYVelocity = 0.3f;
	float divingYVelocity = 1.2f;
	float origMaxYVelocity;
	float yVelocity = 0.1f;
	float maxRotation = 30f;
	float origMaxRotation;
	float divingRotation = 75f;
	float rotationFactor = 5f;
	float divingRotationFactor = 1.5f;
	float origRotationFactor;

	public GameObject environment;
	public GameObject poopPrefab;
	GameObject origPoopPrefab;
	public GameObject pterPoopPrefab;

	public List<Sprite> animationSprites;
	public float animationDelay = 150f;
	AnimationController anim;
	public Sprite deadSprite;
	public Sprite perchedSprite;
	public Sprite poopingSprite;

	List<Sprite> crowAnimationSprites;
	float crowAnimationDelay;
	Sprite crowDeadSprite;
	Sprite crowPerchedSprite;
	Sprite crowPoopingSprite;

	public List<Sprite> toucanAnimationSprites;
	public float toucanAnimationDelay = 150f;
	public Sprite toucanDeadSprite;
	public Sprite toucanPerchedSprite;
	public Sprite toucanPoopingSprite;

	public List<Sprite> pterAnimationSprites;
	public float pterAnimationDelay = 150f;
	public Sprite pterDeadSprite;
	public Sprite pterPerchedSprite;
	public Sprite pterPoopingSprite;

	GameObject perchObject;

	bool flapping = false;
	bool landed = false;
	bool diving = false;
	bool dead = false;
	bool reset = false;
	bool feeding = false;
	bool doublePoints = false;

	int points = 0;
	int availablePoops = 3;
	int totalPoops = 3;

	// Use this for initialization
	void Start () {
		this.origPoopPrefab = poopPrefab;
		this.crowAnimationSprites = animationSprites;
		this.crowAnimationDelay = animationDelay;
		this.crowDeadSprite = deadSprite;
		this.crowPerchedSprite = perchedSprite;
		this.crowPoopingSprite = poopingSprite;

		this.x = transform.position.x;
		this.y = transform.position.y;
		this.origMaxYVelocity = maxYVelocity;
		this.origMaxRotation = maxRotation;
		this.origRotationFactor = rotationFactor;
		this.origYAcceleration = yAcceleration;
		this.origY = y;
		this.anim = new AnimationController (gameObject, animationSprites, animationDelay);
		GameObject.Find ("Poos").GetComponent<Text> ().text = this.availablePoops + "/" + this.totalPoops + " Poos Left";
	}
	
	// Update is called once per frame
	void Update () {
		checkForInput ();
		GameObject.Find ("Poos").GetComponent<Text> ().text = this.availablePoops + "/" + this.totalPoops + " Poos Left";
		if (this.dead) {
			GetComponent<SpriteRenderer> ().sprite = this.deadSprite;
		}else if (!this.landed) {
			this.yVelocity += this.yAcceleration * Time.deltaTime;
			if ((this.yVelocity <= -this.maxYVelocity * 5 && this.yAcceleration < 0)
			    || (this.y + (Mathf.Abs (this.yVelocity * this.yVelocity) / Mathf.Abs (2 * this.origYAcceleration)) > 0.6f)) {
				this.yAcceleration = this.origYAcceleration;
				this.flapping = false;
			}
			if (this.yVelocity > this.maxYVelocity) {
				this.yVelocity -= this.origYAcceleration * Time.deltaTime * 2f;
				this.yVelocity = Mathf.Max (this.yVelocity, this.maxYVelocity);
			}
			if (this.rotationFactor < this.origRotationFactor && !this.diving) {
				this.rotationFactor += this.yAcceleration * Time.deltaTime * 2f;
				this.rotationFactor = Mathf.Min (this.rotationFactor, this.origRotationFactor);
			}
			this.y -= this.yVelocity * Time.deltaTime;
			transform.eulerAngles = new Vector3 (0f, 0f, Mathf.Min (-this.yVelocity / (this.maxYVelocity * rotationFactor) * this.maxRotation, this.maxRotation));
			//FRONT FLIP BIRD LOL: transform.eulerAngles = new Vector3(0f, 0f, -this.yVelocity/this.maxYVelocity * 20);
			setPositionFromXAndY ();
			if (this.diving) {
				GetComponent<SpriteRenderer> ().sprite = this.animationSprites [1];
			} else {
				anim.update ();
			}
		} else {
			this.y = this.perchObject.GetComponent<Collider2D> ().bounds.max.y + 0.07f;
			transform.eulerAngles = new Vector3 (0f, 0f, 0f);
			setPositionFromXAndY ();
			GetComponent<SpriteRenderer> ().sprite = this.perchedSprite;
			if (this.feeding) {
				if (this.perchObject.tag == "Ground") {
					this.availablePoops = this.totalPoops;
				} else if (this.perchObject.tag == "Perch") {
					this.availablePoops += 1;
				}
				this.feeding = false;
				this.availablePoops = Mathf.Min (this.availablePoops, this.totalPoops);
				GameObject.Find ("Poos").GetComponent<Text> ().text = this.availablePoops + "/" + this.totalPoops + " Poos Left";
			}
		}
	}

	private void checkForInput(){
		if (Input.GetButtonDown ("Flap") && !this.flapping && !this.diving) {
			this.yAcceleration = -this.origYAcceleration * 2f;
			this.rotationFactor = this.origRotationFactor;
			this.flapping = true;
			this.landed = false;
		}
		if (Input.GetButton ("Dive") && !this.landed && !this.flapping) {
			this.maxYVelocity = divingYVelocity;
			this.maxRotation = divingRotation;
			this.rotationFactor = divingRotationFactor;
			this.diving = true;
		}else if (!Input.GetButton ("Dive") && !this.landed && !this.flapping && this.diving) {
			this.maxYVelocity = this.origMaxYVelocity;
			this.maxRotation = this.origMaxRotation;
			this.diving = false;
		}
		if (Input.GetButtonDown ("Poop") && !this.landed && !this.dead) {
			this.poop ();
		}
		if ((Input.GetButtonDown ("Flap") || Input.GetButtonDown ("Dive")) && this.dead) {
			this.reset = true;
		}
		if (Input.GetButtonDown ("Mute")) {
			GameObject.Find ("Audio Source").GetComponent<AudioSource> ().mute = !GameObject.Find ("Audio Source").GetComponent<AudioSource> ().mute;
		}
	}

	private void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Perch" || coll.gameObject.tag == "Ground") {
			this.landed = true;
			this.diving = false;
			this.flapping = false;
			this.feeding = true;
			this.maxYVelocity = origMaxYVelocity;
			this.maxRotation = origMaxRotation;
			this.rotationFactor = origRotationFactor;
			this.yAcceleration = origYAcceleration;
			this.yVelocity = 0f;
			this.perchObject = coll.gameObject;
		} else if (coll.gameObject.tag == "HurtOnTouch") {
			this.landed = true;
			this.diving = false;
			this.flapping = false;
			this.dead = true;
			this.maxYVelocity = origMaxYVelocity;
			this.maxRotation = origMaxRotation;
			this.rotationFactor = origRotationFactor;
			this.yAcceleration = origYAcceleration;
			this.yVelocity = 0f;
			this.perchObject = coll.gameObject;
		}
	}

	private void setPositionFromXAndY (){
		transform.position = new Vector2 (roundToPixel (this.x), roundToPixel (this.y));
	}

	private float roundToPixel(float x){
		Camera currentCamera = Camera.allCameras [0];
		float pixelsPerUnit = currentCamera.pixelHeight / (currentCamera.orthographicSize * 2);
		float roundedX = Mathf.RoundToInt (x * pixelsPerUnit) / pixelsPerUnit;
		return roundedX;
	}

	private void poop() {
		if (this.availablePoops > 0) {
			GetComponent<SpriteRenderer> ().sprite = this.poopingSprite;
			GameObject.Instantiate (poopPrefab, transform.position - new Vector3 (0.04f, 0.1f), new Quaternion ());
			this.availablePoops -= 1;
			GameObject.Find ("Poos").GetComponent<Text> ().text = this.availablePoops + "/" + this.totalPoops + " Poos Left";
		}
	}

	public bool isLanded(){
		return landed;
	}

	public bool isDead(){
		return dead;
	}

	public bool isReset(){
		return reset;
	}

	public void addPoints(int points){
		this.points += points;
		GameObject pts = GameObject.Find ("Points");
		if (pts != null) {
			if (this.doublePoints) {
				this.points += points;
			}
			pts.GetComponent<Text> ().text = this.points + " Seeds";
		}
			
	}

	public int getPoints(){
		return this.points;
	}

	public void setPoints(int points){
		this.points = points;
	}

	public void Respawn(){
		this.availablePoops = this.totalPoops;
		this.y = this.origY;
		this.yVelocity = 0f;
		this.yAcceleration = this.origYAcceleration;
		this.maxRotation = this.origMaxRotation;
		this.rotationFactor = this.origRotationFactor;
		this.dead = false;
		this.landed = false;
		this.flapping = false;
		this.reset = false;
		GameObject.Find ("Poos").GetComponent<Text> ().text = this.availablePoops + "/" + this.totalPoops + " Poos Left";
	}

	public void setBird(string bird){
		if (bird == "toucan") {
			animationSprites = toucanAnimationSprites;
			animationDelay = toucanAnimationDelay;
			deadSprite = toucanDeadSprite;
			perchedSprite = toucanPerchedSprite;
			poopingSprite = toucanPoopingSprite;
			poopPrefab = origPoopPrefab;
			this.totalPoops = 5;
			this.availablePoops = 5;
			transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			this.doublePoints = true;
		} else if (bird == "crow") {
			animationSprites = crowAnimationSprites;
			animationDelay = crowAnimationDelay;
			deadSprite = crowDeadSprite;
			perchedSprite = crowPerchedSprite;
			poopingSprite = crowPoopingSprite;
			poopPrefab = origPoopPrefab;
			this.totalPoops = 3;
			this.availablePoops = 3;
			this.doublePoints = false;
			transform.localScale = new Vector3 (1f, 1f, 1f);
		} else if (bird == "pter") {
			animationSprites = pterAnimationSprites;
			animationDelay = pterAnimationDelay;
			deadSprite = pterDeadSprite;
			perchedSprite = pterPerchedSprite;
			poopingSprite = pterPoopingSprite;
			poopPrefab = pterPoopPrefab;
			this.totalPoops = 50;
			this.availablePoops = 50;
			transform.localScale = new Vector3 (1f, 1f, 1f);
			this.doublePoints = false;
		}
		this.anim = new AnimationController (gameObject, animationSprites, animationDelay);
	}
}
