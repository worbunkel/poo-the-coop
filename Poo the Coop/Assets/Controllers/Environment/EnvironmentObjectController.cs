using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObjectController : MonoBehaviour {
	void Start () {
		
	}

	void Update () {
	}

	void OnBecameInvisible() {
		Destroy (gameObject);
	}
}
