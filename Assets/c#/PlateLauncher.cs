using UnityEngine;
using System.Collections;

public class PlateLauncher : MonoBehaviour {
	[SerializeField] public Transform leftLauncher;
	[SerializeField] public Transform rightLauncher;
	[SerializeField] public Transform middlePoint;
	[SerializeField] public Transform platePref;
	[SerializeField] public GameObject startBtn;
	private Transform[] fromPos;
	public static PlateLauncher instance;

	void Awake(){
		if (instance == null)
			instance = this;
	}

	void Start () {
		fromPos = new Transform[2] {leftLauncher, rightLauncher};
	}

	public void GoPlate(){
		DeactivateBtn ();
		Transform plate = Instantiate (platePref) as Transform;
		plate.SetParent (this.transform);
		int from = Random.Range (0, 2);
		plate.GetComponent<Plate> ().startPos = fromPos[from].position;
		plate.GetComponent<Plate> ().endPos = (from==0) ? fromPos[1].position: fromPos[0].position;
		plate.GetComponent<Plate> ().controlPos = middlePoint.position;
	}

	public void DeactivateBtn(){
		startBtn.SetActive (false);
		// здесь можно активировать джостик
	}

	public void ActivateBtn(){
		startBtn.SetActive (true);
	}
}
