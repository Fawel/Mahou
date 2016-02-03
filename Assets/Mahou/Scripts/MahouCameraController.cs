using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MahouCameraController : MonoBehaviour
{
	public Text rjVText;
	public Text rjHText;

	[SerializeField]
	private float distanceAway;
	[SerializeField]
	private float distanceUp;
	[SerializeField]
	private float smooth;
	[SerializeField]
	private Transform followXForm;
	[SerializeField]
	private Vector3 offset = new Vector3 (0f, 1.5f, 0f);

	private Vector3 targetPosition;
	private Vector3 lookDir;
	float h = 0.0f;
	float v = 0.0f;
	Vector3 inputOffset_prev = Vector3.zero;
	private Vector3 velocityCamSmooth = Vector3.zero;
	[SerializeField]
	private float camSmoothDampTime = 0.1f;
	void Start()
	{
		followXForm = GameObject.FindWithTag ("Player").transform;	
	}

	void Update()
	{

	}

	void FixedUpdate ()
	{

	}

	void LateUpdate (){
		h = Input.GetAxis("RJHorizontal")*15f;				
		v = Input.GetAxis("RJVertical")*0.1f;
		rjHText.text = h.ToString ();
		rjVText.text = v.ToString ();
		offset.y += v;
		offset.y = Mathf.Clamp (offset.y, -2f, 2f);
		Quaternion rotation = Quaternion.Euler (v, h, 0);

		Vector3 characterOffset = followXForm.position + offset;
		lookDir = characterOffset - this.transform.position;
		lookDir.y = 0;
		lookDir = rotation * lookDir;
		lookDir.Normalize ();

		Vector3 negDistance = new Vector3(0.0f, 0.0f, -distanceAway);
		targetPosition = characterOffset - lookDir * distanceAway;
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