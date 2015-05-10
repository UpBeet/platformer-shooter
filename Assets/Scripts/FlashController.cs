using UnityEngine;
using System.Collections;

public class FlashController : MonoBehaviour {

	public Sprite[] flashSprites;
	public int flashFrames = 2;

	private SpriteRenderer currentFlash = null;
	private int frameCount = 0;

	// Update is called once per frame
	void Update () {
		if (currentFlash != null) {
			frameCount++;
			if (frameCount >= flashFrames) {
				Destroy (currentFlash.gameObject);
				currentFlash = null;
				frameCount = 0;
			}
		}
	}

	public void Flash () {
		if (currentFlash == null) {
			currentFlash = new GameObject (this.name + " Flash").AddComponent<SpriteRenderer> ();;
			currentFlash.transform.position = transform.position;
			currentFlash.transform.parent = transform;
		}
		currentFlash.sprite = PickRandomFlashSprite ();
		frameCount = 0;
	}

	private Sprite PickRandomFlashSprite () {
		int count = flashSprites.Length;
		int index = Random.Range (0, count - 1);
		return flashSprites[index];
	}
}
