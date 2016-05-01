using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerCTRL : NetworkBehaviour {
	//Player
	[SerializeField] private int amountKnife = 0;
	[SyncVar] private Animator anim;

	private bool faceRight;

	//bullet
	public GameObject knifePrefab;
	[SyncVar] private Transform spawnPoint;
	private Rigidbody2D knifeRB;

	void Start(){
		amountKnife = 10;
		faceRight = true;
		anim = GetComponent<Animator> ();
		knifeRB = knifePrefab.GetComponent<Rigidbody2D> ();
		spawnPoint = transform.FindChild ("SpawnKnife");
	}

	void Update(){
		if (isLocalPlayer) {
			//Test IncreaseKnife
			if (Input.GetKeyDown (KeyCode.R)) {
				amountKnife++;
			}

			//PlayerCtrl
			if (Input.GetKeyDown (KeyCode.A)) {
				anim.SetTrigger ("Attack");
			}
			if (Input.GetKeyDown (KeyCode.S)) {
				if (amountKnife > 0) {
					anim.SetTrigger ("Throw");
					CmdThrowKnife ();
					amountKnife--;
				}
			}
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				anim.SetFloat ("Speed", 0.5f);
				faceRight = true;
				CmdFacing ();
			}
			if (Input.GetKeyUp (KeyCode.RightArrow)) {
				anim.SetFloat ("Speed", 0f);
			}
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				anim.SetFloat ("Speed", 0.5f);
				faceRight = false;
				CmdFacing ();
			}
			if (Input.GetKeyUp (KeyCode.LeftArrow)) {
				anim.SetFloat ("Speed", 0f);
			}
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				anim.SetFloat ("Speed", 0.5f);
			}
			if (Input.GetKeyUp (KeyCode.UpArrow)) {
				anim.SetFloat ("Speed", 0f);
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				anim.SetFloat ("Speed", 0.5f);
			}
			if (Input.GetKeyUp (KeyCode.DownArrow)) {
				anim.SetFloat ("Speed", 0f);
			}
			if (Input.GetKeyDown (KeyCode.LeftShift)) {
				anim.SetFloat ("Speed", 1f);
			}
			if (Input.GetKeyUp (KeyCode.LeftShift)) {
				anim.SetFloat ("Speed", 0f);
			}
		}
	}

	[Command]
	void CmdFacing(){
		RpcFacing (faceRight);
	}

	[ClientRpc]
	void RpcFacing(bool faceR){
		if (faceR == true) {
			transform.eulerAngles = new Vector2 (0, 0);
		}
		if (faceR == false) {
			transform.eulerAngles = new Vector2 (0, 180);
		}
	}

	[Command]
	void CmdThrowKnife(){
		if (faceRight == true) {
			GameObject tmp = (GameObject)Instantiate (knifePrefab, spawnPoint.transform.position, Quaternion.Euler(new Vector3(0,0,-90)));
			NetworkServer.Spawn (tmp);
			tmp.GetComponent<Knife> ().Initialize (Vector2.right);
		}
		else if (faceRight == false) {
			GameObject tmp = (GameObject)Instantiate (knifePrefab, spawnPoint.transform.position, Quaternion.Euler(new Vector3(0,0,90)));
			NetworkServer.Spawn (tmp);
			tmp.GetComponent<Knife> ().Initialize (Vector2.left);
		}
	}

}
