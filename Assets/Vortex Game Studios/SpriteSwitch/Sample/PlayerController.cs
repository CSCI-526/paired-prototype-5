using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private Animator _animator = null;
	private bool _isWalking = false;
	// Use this for initialization
	void Start () {
		if ( _animator == null ) {
			_animator = GetComponent<Animator>();
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if ( !_isWalking ) {
			if ( Input.GetAxisRaw( "Vertical" ) > 0.0f ) {
				_animator.SetTrigger( "lookUp" );
			} else if ( Input.GetAxisRaw( "Vertical" ) < 0.0f ) {
				_animator.SetTrigger( "lookDown" );
			} else if ( Input.GetAxisRaw( "Horizontal" ) > 0.0f ) {
				_animator.SetTrigger( "lookRight" );
			} else if ( Input.GetAxisRaw( "Horizontal" ) < 0.0f ) {
				_animator.SetTrigger( "lookLeft" );
			}
		}

		if ( Input.GetAxisRaw( "Horizontal" ) != 0.0f || Input.GetAxisRaw( "Vertical" ) != 0.0f ) {
			_isWalking = true;
		} else {
			_isWalking = false;
		}

		_animator.SetBool( "isWalking", _isWalking );
	}
}
