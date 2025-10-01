using UnityEngine;
using System.Collections;

public class SwitchByCode : MonoBehaviour {
	public SpriteSwitch spriteSwitch;

	public Texture2D guyTexture;
	public Texture2D girlTexture;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickGuy() {
		spriteSwitch.spriteTexture = guyTexture;
	}

	public void OnClickGirl() {
		spriteSwitch.spriteTexture = girlTexture;
	}
}
