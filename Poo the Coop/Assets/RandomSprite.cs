using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour {

	public List<Sprite> possibleSprites;
	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer> ().sprite = possibleSprites [Random.Range (0, possibleSprites.Count - 1)];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
