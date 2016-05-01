using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour {

	private float maxHealth = 100f;
	[SyncVar] private float currentHealth = 0f;
	[SyncVar] public GameObject hpBar;
	private Animator anim;
	[SyncVar] private bool DieStat;

	void Start () {
		currentHealth = maxHealth;
		anim = GetComponent<Animator> ();
		DieStat = false;
	}

	//Hit Animate
	void SetAnimationHit(){
		anim.SetTrigger ("Hit");
	}

	//Die Animate
	void SetAnimationDie(){
		anim.SetTrigger ("Die");
	}

	public void setHP(float myHP){
		hpBar.transform.localScale = new Vector2 (myHP,hpBar.transform.localScale.y);
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.K)) {
			CmdDecreaseHP();
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Knife") {
			CmdDecreaseHP ();
			Destroy (col.gameObject);

			if (currentHealth <= 0) {
				currentHealth = 0;
				DieStat = true;
				SetAnimationDie ();
				gameObject.SetActive (false);
			}
		}
	}

	[Command]
	public void CmdDecreaseHP(){
		if (DieStat == true) {
			return;
		} else {
			RpcHit ();
		}
	}


	[ClientRpc]
	public void RpcHit(){
		currentHealth -= 2;
		SetAnimationHit ();

		float calc_hp = currentHealth / maxHealth;
		setHP (calc_hp);
	}

}
