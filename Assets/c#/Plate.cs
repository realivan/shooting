using UnityEngine;
using System.Collections;

public class Plate : MonoBehaviour {
	[SerializeField] public Transform explosionPrefab; 
	public Vector3 startPos;
	public Vector3 endPos;
	public Vector3 controlPos;
	private Transform thisTransform;
	public float lerpTime = 5f;
	private float covDist;
	private float way;
	private float speed;
	public float startTime;

	void Start () {
		thisTransform = transform;
		startTime = Time.time;
		way = Vector3.Distance (startPos, endPos);
		speed = way / lerpTime;
		thisTransform.position = startPos;
	}

	void Update () {
		Fly ();
	}

	private void Fly(){
		covDist = (Time.time - startTime) * speed;
		float f = covDist / way;
		Vector3 m1 = Vector3.Lerp(startPos, controlPos, f );
		Vector3 m2 = Vector3.Lerp(controlPos, endPos, f );
		thisTransform.position = Vector3.Lerp (m1, m2, f);
		if (way - covDist < 1f) {
			DestroyPlate();
		}
	}

	public void DestroyPlate(bool shoot = false){
		if (shoot) {
			Transform explosion = Instantiate (explosionPrefab) as Transform;
			explosion.position = thisTransform.position;
		}
		PlateLauncher.instance.ActivateBtn ();
		Destroy (this.gameObject);
	}
}
