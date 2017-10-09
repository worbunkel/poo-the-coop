using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseBirdController : MonoBehaviour {

	int seeds;
	List<string> ownedBirds;
	string selectedBird;
	public Material normalSpriteMat;
	void Start () {
		ownedBirds = new List<string> ();
		ownedBirds.Add ("crow");
	}

	void Update() {
		GameObject.Find ("ChooseSeeds").GetComponent<Text>().text = seeds + " Seeds";
	}

	public void setBird(BirdController bird){
		seeds = bird.getPoints ();
	}

	public void selectCrow(){
		selectedBird = "crow";
	}

	public void selectOrBuyToucan(){
		if (seeds >= 200 && !ownedBirds.Contains ("toucan")) {
			ownedBirds.Add ("toucan");
			seeds -= 200;
			GameObject.Find ("Toucan").GetComponent<SpriteRenderer> ().material = normalSpriteMat;
		}
		if (ownedBirds.Contains ("toucan")) {
			selectedBird = "toucan";
		}
	}

	public void selectOrBuyPter(){
		if (seeds >= 2000 && !ownedBirds.Contains ("pter")) {
			ownedBirds.Add ("pter");
			seeds -= 2000;
			GameObject.Find ("Pter").GetComponent<SpriteRenderer> ().material = normalSpriteMat;
		}
		if (ownedBirds.Contains ("pter")) {
			selectedBird = "pter";
		}
	}

	public List<string> getOwnedBirds(){
		return this.ownedBirds;
	}

	public string getSelectedBird(){
		return selectedBird;
	}

	public void goBackToGame(){
		GameObject.Find ("SceneHolder").GetComponent<SceneController> ().GoToPlay (this.selectedBird, seeds);
	}
}
