using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class SpriteSwitch : MonoBehaviour {
	private Material _spriteSwitchMaterial = null;

    //[SerializeField]
    private Texture2D _mainSprite = null;

    [SerializeField]
    private Texture2D _switchSprite = null;
	public Texture2D spriteTexture {
		get { return _switchSprite; }
		set {
			_switchSprite = value;

			if ( _switchSprite == null ) {
				_switchSprite = _mainSprite;
			} else {
				if ( _spriteSwitchMaterial == null )
					_spriteSwitchMaterial = GetComponent<Renderer>().material = new Material( Shader.Find( "Vortex Game Studios/SpriteSwitch" ) );
				_spriteSwitchMaterial.SetTexture( "_SwitchTex", _switchSprite );
			}
		}
	}

	// Use this for initialization
	void Start () {
		if ( _mainSprite == null && GetComponent<SpriteRenderer>() ) {
			_mainSprite = GetComponent<SpriteRenderer>().sprite.texture;
		} else {
			_mainSprite = null;
		}

		spriteTexture = _switchSprite;
	}

    void OnValidate() {
		if ( _mainSprite == null && GetComponent<SpriteRenderer>() ) {
			_mainSprite = GetComponent<SpriteRenderer>().sprite.texture;
		} else {
			_mainSprite = null;
		}

        spriteTexture = _switchSprite;
    }
}
