using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	void Start () {
		StartCoroutine (Wait ());
	}
	
	IEnumerator Wait(){
		yield return new WaitForSeconds (2f);
		Destroy (this.gameObject);
	}
}
