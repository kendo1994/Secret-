using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody2D))]

public class Knife : NetworkBehaviour {

	[SyncVar] private int speed = 20;

	[SyncVar] private Vector2 direction;
	private Rigidbody2D rb2d;

	void Start(){
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate(){
		rb2d.velocity = direction * speed;
	}

	public void Initialize (Vector2 direct){
		this.direction = direct;
	}

	void OnBecameInvisible(){
		Destroy (gameObject);
	}
		
}
