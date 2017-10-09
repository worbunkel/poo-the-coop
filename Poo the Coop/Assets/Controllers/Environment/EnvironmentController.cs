using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour {

	public GameObject bird;
	BirdController birdController;
	public GameObject groundPrefab;
	List<GameObject> existingObjects;
	List<GameObject> existingGrounds;

	public GameObject housePrefab;
	public float houseSpawnChance = 75f;

	public GameObject treePrefab;
	public float treeSpawnChance = 80f;

	public GameObject carPrefab;
	public float carSpawnChance = 20f;

	public GameObject truckPrefab;
	public float truckSpawnChance = 10f;

	public GameObject cloudPrefab;
	public float cloudSpawnChance = 30f;

	public GameObject catPrefab;
	public float catSpawnChance = 25f;

	public GameObject dogPrefab;
	public float dogSpawnChance = 25f;

	public GameObject ufoPrefab;
	public float ufoSpawnChance = 5f;

	public GameObject guy1Prefab;
	public float guy1SpawnChance = 25f;

	Dictionary<GameObject, float> spawnablePrefabs;

	public float speed = 0.5f;

	float lastUpdateTime;

	void Start () {
		existingGrounds = new List<GameObject>();
		this.birdController = bird.GetComponent<BirdController> ();
		this.spawnablePrefabs = new Dictionary<GameObject, float> ();
		this.spawnablePrefabs.Add (housePrefab, houseSpawnChance);
		this.spawnablePrefabs.Add (catPrefab, catSpawnChance);
		this.spawnablePrefabs.Add (dogPrefab, dogSpawnChance);
		lastUpdateTime = Time.time;

		generateNextChunk ();
	}

	void Update () {
		this.speed += 0.01f * Time.deltaTime;
		if (this.birdController.isReset()) {
			this.speed = 0.5f;
			GameObject.Find ("SceneHolder").GetComponent<SceneController> ().GoToChoose (this.birdController);
		}
		if (this.birdController.isDead ()) {
			Time.timeScale = 0f;
		}
		if (!this.birdController.isLanded() && !this.birdController.isDead()) {
			transform.position -= new Vector3 (speed, 0, 0) * Time.deltaTime;
		}
		if (existingGrounds.Count == 0) {
			generateNextChunk ();
		}
		GameObject lastGround = existingGrounds [existingGrounds.Count - 1];
		if (lastGround.transform.position.x < 3f) {
			generateNextChunk ();
		}
		if (Time.time - lastUpdateTime > 1) {
			spawnMovingPrefabs ();
			lastUpdateTime = Time.time;
		}

	}

	public void startPlaying(string selectedBird) {
		foreach (GameObject ground in existingGrounds) {
			Destroy (ground);
		}
		existingGrounds = new List<GameObject> ();
		this.birdController.setBird (selectedBird);
		this.birdController.Respawn ();
		Time.timeScale = 1f;
	}

	void generateNextChunk() {
		GameObject ground = GameObject.Instantiate (groundPrefab, transform);
		foreach(KeyValuePair<GameObject, float> spawnablePrefab in spawnablePrefabs){
			spawnGeneric (ground, spawnablePrefab.Key, spawnablePrefab.Value);
		}
		spawnTree (ground);
		if (existingGrounds.Count > 0) {
			GameObject lastGround = existingGrounds [existingGrounds.Count - 1];
			ground.transform.position = new Vector3 (ground.GetComponent<SpriteRenderer> ().bounds.size.x, 0, 0) + lastGround.transform.position;
		}
		existingGrounds.Add (ground);
	}

	void spawnMovingPrefabs() {
		if (existingGrounds.Count > 0) {
			GameObject ground = existingGrounds [existingGrounds.Count - 1];
			float rand = Random.value * 100f;
			if (rand < ufoSpawnChance) {
				GameObject.Instantiate (ufoPrefab, ground.transform);
			}
			rand = Random.value * 100f;
			if (rand < guy1SpawnChance) {
				GameObject.Instantiate (guy1Prefab, ground.transform);
			}
			if (rand < truckSpawnChance) {
				GameObject.Instantiate (truckPrefab, ground.transform);
			} else if (rand - truckSpawnChance < carSpawnChance) {
				GameObject.Instantiate (carPrefab, ground.transform);
			}
			if (rand < cloudSpawnChance){
				GameObject obj = GameObject.Instantiate (cloudPrefab, ground.transform);
				obj.transform.position -= new Vector3 (0, Random.value * 0.2f, 0);
			}
		}
	}

	void spawnGeneric(GameObject ground, GameObject prefab, float spawnChance){
		if (Random.value * 100f <= spawnChance) {
			float groundWidth = ground.GetComponent<SpriteRenderer> ().bounds.size.x;
			GameObject obj = GameObject.Instantiate (prefab, ground.transform);
			float objWidth = obj.GetComponent<SpriteRenderer> ().bounds.size.x;
			float maxShift = (groundWidth - objWidth);
			float objX = ground.transform.position.x - maxShift / 2f;
			objX += Random.value * maxShift;
			obj.transform.position = new Vector3 (objX, obj.transform.position.y, obj.transform.position.z);
		}
	}

	void spawnTree(GameObject ground){
		if (Random.value * 100f <= treeSpawnChance) {
			float groundWidth = ground.GetComponent<SpriteRenderer> ().bounds.size.x;
			GameObject obj = GameObject.Instantiate (treePrefab, ground.transform);
			float objWidth = obj.GetComponent<SpriteRenderer> ().bounds.size.x;
			float maxShift = (groundWidth - objWidth);
			float objX = ground.transform.position.x - maxShift / 2f;
			objX += Random.value * maxShift;
			obj.transform.position = new Vector3 (objX, obj.transform.position.y, obj.transform.position.z);
			Transform houseTransform = ground.transform.Find ("House(Clone)");
			if (houseTransform != null) {
				while (Mathf.Abs (obj.transform.localPosition.x - houseTransform.localPosition.x) < 0.15f) {
					obj.transform.localPosition = new Vector3(obj.transform.localPosition.x * 1.2f + 0.001f, obj.transform.localPosition.y, obj.transform.localPosition.z);
				}
			}
		}
	}
}
