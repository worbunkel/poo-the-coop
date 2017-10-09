using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoToChoose(BirdController bird){
		transform.GetChild (0).gameObject.SetActive (false);
		transform.GetChild (1).gameObject.SetActive (true);
		GameObject.Find ("ChooseYourBird").GetComponent<ChooseBirdController> ().setBird (bird);
	}

	public void GoToPlay(string selectedBird, int points){
		transform.GetChild (0).gameObject.SetActive (true);
		transform.GetChild (1).gameObject.SetActive (false);
		GameObject.Find ("Environment").GetComponent<EnvironmentController> ().startPlaying (selectedBird);
		GameObject.Find ("Bird").GetComponent<BirdController> ().setPoints (points);
	}
}
