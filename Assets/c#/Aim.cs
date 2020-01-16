using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Aim : MonoBehaviour {
	[SerializeField] public Image aimImg;
	private Plate plateAim;
	private bool lastSecond;

	public bool underAim;
	public float underAimTime;	// время нахождения под прицелом
	private float startUnderAimTime;

	void Start () {

	}

	void Update(){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (new Vector3(Screen.width*0.5f, Screen.height*0.5f, 0));
		Debug.DrawRay (ray.origin, ray.direction * 500, Color.red);
		if (Physics.Raycast (transform.position, ray.direction, out hit, 100)) {
			if (hit.collider.GetComponent<Plate> () == null)
				return;
			if (plateAim == null) {
				plateAim = hit.collider.GetComponent<Plate> ();
				GetTarget ();
			} else{
				FillImg ();
				Targering ();
			}
		} else {
			if (plateAim != null){
				LostTarget ();
				plateAim = null;
			}
			RefreshFill ();
		}
	}

	private void FillImg(){
		aimImg.fillAmount += Time.deltaTime / underAimTime;
	}

	private void RefreshFill(){
		aimImg.fillAmount = 0;
	}

	public void Targering(){
		float aimTime = Time.time - startUnderAimTime;	// время нахождения в прицел
		if(lastSecond && Time.time - plateAim.startTime <=0.1f){
			int n = Random.Range (0, 100);
			Debug.Log (n);
			int percent = (int)((aimTime-startUnderAimTime)*10);
			Debug.Log (percent);
			// процент попадания
			if(percent >= n)
				plateAim.DestroyPlate (true);
		}else if (aimTime >= underAimTime) {
			plateAim.DestroyPlate (true);
		}
	}
	
	public void GetTarget(){
		startUnderAimTime = Time.time;
		underAim = true;
		float pastTime = startUnderAimTime - plateAim.startTime; // всего прошло время с начала полета
		if (pastTime < 1)
			underAimTime = 0.25f;
		else if (pastTime < 2)
			underAimTime = 0.5f;
		else if (pastTime < 3)
			underAimTime = 0.75f;
		else if (pastTime < 4)
			underAimTime = 1f;
		else if (pastTime < 5) {
			underAimTime = 1f;
			lastSecond = true;
			Debug.Log ("Lucky srike");
		}
	}
	
	public void LostTarget(){
		startUnderAimTime = 0;
		underAimTime = 0;
		lastSecond = false;
		underAim = false;
	}
}
