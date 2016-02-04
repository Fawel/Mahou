using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MahouCameraController : MonoBehaviour
{
	public Text rjVText;
	public Text rjHText;
	private ShoujoController follow;
	[SerializeField]
	private float distanceAway;
	[SerializeField]
	private float distanceUp;
	[SerializeField]
	private float smooth;
	[SerializeField]
	private Transform followXForm;
	private Transform target;
	[SerializeField]
	private Vector3 offset = new Vector3 (0f, 1.5f, 0f);

	private Vector3 targetPosition;
	private Vector3 lookDir;
	private Vector3 curLookDir;
	private Vector3 velocityLookDir = Vector3.zero;
	private float lookDirDampTime = 0.1f;
	float h = 0.0f;
	float v = 0.0f;
	float leftX;
	float leftY;
	Vector3 inputOffset_prev = Vector3.zero;
	private Vector3 velocityCamSmooth = Vector3.zero;
	[SerializeField]
	private float camSmoothDampTime = 0.1f;
	private CamStates camState = CamStates.Free;
	public Text camStateText;
	public enum CamStates
	{
		Free,
		Target
	}


	void Start()
	{
		follow = GameObject.FindWithTag("Unitychan").GetComponent<ShoujoController>();
		followXForm = GameObject.FindWithTag ("Player").transform;	
		lookDir = followXForm.forward;
		curLookDir = followXForm.forward;
	}

	void Update()
	{

	}

	void FixedUpdate ()
	{
		target = GameObject.FindWithTag ("Target").transform;	
	}

	void LateUpdate (){
		h = Input.GetAxis("RJHorizontal")*15f;				
		v = Input.GetAxis("RJVertical")*0.1f;
		leftX = Input.GetAxis("LJHorizontal");				
		leftY = Input.GetAxis("LJVertical");
		rjHText.text = h.ToString ();
		rjVText.text = v.ToString ();
		offset.y += v;
		offset.y = Mathf.Clamp (offset.y, -1f, 2f);
		Quaternion rotation = Quaternion.Euler (v, h, 0);

		if (Input.GetButtonDown("RJButton")) {
			if (camState == CamStates.Free && target != null) {
				camState = CamStates.Target;
				follow.Animator.SetBool ("Targeting", true);
				camStateText.text = "Target";
			} 
			else {
				camState = CamStates.Free;
				follow.Animator.SetBool ("Targeting", false);
				camStateText.text = "Free";
			}
		}
		Vector3 characterOffset = followXForm.position + offset;
		Vector3 negDistance = new Vector3(0.0f, 0.0f, -distanceAway);
		switch (camState) {
		case CamStates.Free:
			
			camSmoothDampTime = 0.1f;
			lookDir = characterOffset - this.transform.position;
			lookDir.y = 0;
			lookDir = rotation * lookDir;
			lookDir.Normalize ();
			targetPosition = characterOffset - lookDir * distanceAway;
			break;
		case CamStates.Target:
			camSmoothDampTime = 0.05f;
			Vector3 targetOffset = target.position;
			lookDir = targetOffset - this.transform.position;
			lookDir.y = 0;
			lookDir = rotation * lookDir;
			lookDir.Normalize ();

			targetPosition = characterOffset - lookDir * distanceAway;
			break;
		}
		CompensateForWalls (characterOffset, ref targetPosition);
		SmoothPosition(this.transform.position, targetPosition);
		transform.LookAt (followXForm);
		Debug.DrawRay (transform.position, lookDir, Color.red);
	}

	private void SmoothPosition(Vector3 fromPos, Vector3 toPos)
	{
		this.transform.position = Vector3.SmoothDamp (fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
	}

	private void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget)
	{
		Debug.DrawLine (fromObject, toTarget, Color.cyan);

		RaycastHit wallHit = new RaycastHit ();
		if (Physics.Linecast(fromObject,toTarget, out wallHit)) 
		{
			Debug.DrawRay (wallHit.point, Vector3.left, Color.red);
			toTarget = new Vector3 (wallHit.point.x, toTarget.y, wallHit.point.z);
		}
	}
}