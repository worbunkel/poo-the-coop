using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : Object {

	GameObject go;
	List<Sprite> sprites;
	float delay, updateTime;
	int spriteIndex = 0;

	public AnimationController(GameObject go, List<Sprite> sprites, float delay){
		this.sprites = sprites;
		this.delay = delay;
		this.go = go;
		this.updateTime = Time.time * 1000;
	}

	public void update(){
		if (Time.time * 1000 - this.updateTime > delay) {
			spriteIndex += 1;
			spriteIndex = spriteIndex % this.sprites.Count;
			this.go.GetComponent<SpriteRenderer> ().sprite = sprites [spriteIndex];
			this.updateTime = Time.time * 1000;
		}
	}
}
