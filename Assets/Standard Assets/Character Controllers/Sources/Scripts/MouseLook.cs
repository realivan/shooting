using UnityEngine;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	float sensitivityX = 1F;
	float sensitivityY = 1F;

	private float minimumX = -60F;
	private float maximumX = 60F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;
	float rotationX = 0f;

    private Touch _initialTouch;
    private bool _hasSwiped;
    private float _swipeDist;

    void Update ()	{

#if UNITY_EDITOR
        sensitivityX = sensitivityY = 4f;
        if (axes == RotationAxes.MouseXAndY) {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        } else if (axes == RotationAxes.MouseX) {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        } else {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
#endif

#if UNITY_ANDROID
        if (Input.touchCount == 0)
            return;
        foreach (Touch t in Input.touches) {
            if (t.phase == TouchPhase.Began) {
                _initialTouch = t;
            } else if (t.phase == TouchPhase.Moved && !_hasSwiped) {
                float deltaX = _initialTouch.position.x - t.position.x;
                float deltaY = _initialTouch.position.y - t.position.y;
                _swipeDist = Mathf.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
                if (_swipeDist > 10f) {
                    rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                    rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
                    rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);
                    transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
                }
            } else if (t.phase == TouchPhase.Ended) {
                _initialTouch = new Touch();
                _hasSwiped = false;
            }
        }
#endif
    }

    void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
	}
}