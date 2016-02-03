using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShoujoController : MonoBehaviour {
	[SerializeField] private Animator animator;
	[SerializeField] private float directionDampTime = .25f;
	[SerializeField] float m_GroundCheckDistance = 0.1f;
	[SerializeField] float m_MovingTurnSpeed = 360;
	[SerializeField] float m_StationaryTurnSpeed = 180;
	[SerializeField] float m_MoveSpeedMultiplier = 1f;
	[SerializeField] float m_AnimSpeedMultiplier = 1f;
	[SerializeField] float directionSpeed = 3f;
	[SerializeField] private Rigidbody m_Rigidbody;
	bool m_IsGrounded;
	float runSpeedMultiplier = 6f;
	private float speed = 0.0f;
	private float direction = 0.0f;
	private float h = 0.0f;
	private float v = 0.0f;
	float m_TurnAmount;
	float m_ForwardAmount;
	private Transform m_Cam; 
	private Vector3 m_CamForward; 
	private Vector3 m_Move;
	Vector3 m_GroundNormal;
	public Text ljVText;
	public Text ljHText;
	public Text turnText;

	void Start(){
		m_Cam = Camera.main.transform;
		animator = GetComponent<Animator>();
		m_Rigidbody = GetComponent<Rigidbody>();
		if (animator.layerCount >= 2) {
			animator.SetLayerWeight(1, 1);
		}
	}

	void Update(){
		
	}

	void FixedUpdate(){
		if (animator) {
			h = Input.GetAxis("LJHorizontal");
			v = Input.GetAxis("LJVertical");
			ljHText.text = h.ToString ();
			ljVText.text = v.ToString ();

			//StickToWorldSpace (this.transform, m_Cam, ref directionDampTime, ref speed);

			/*animator.SetFloat("Speed", speed);
			animator.SetFloat("Direction", direction, directionDampTime, Time.deltaTime);*/
		}

		if (m_Cam != null)
		{
			// calculate camera relative direction to move:
			m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
			m_Move = v*m_CamForward + h*m_Cam.right;
		}
		else
		{
			// we use world-relative directions in the case of no main camera
			m_Move = v*Vector3.forward + h*Vector3.right;
		}

		if (m_Move.magnitude > 1f) m_Move.Normalize();
		m_Move = transform.InverseTransformDirection(m_Move);
		CheckGroundStatus();
		m_Move = Vector3.ProjectOnPlane(m_Move, m_GroundNormal);
		m_TurnAmount = Mathf.Atan2(m_Move.x, m_Move.z);

		turnText.text = m_TurnAmount.ToString();

		m_ForwardAmount = m_Move.z;
		if (m_ForwardAmount == 0) {
			m_TurnAmount = 0;
		}
		ApplyExtraTurnRotation();
		UpdateAnimator(m_Move);

	}
	void OnAnimatorMove() {
		// we implement this function to override the default root motion.
		// this allows us to modify the positional speed before it's applied.
		//if (m_IsGrounded && Time.deltaTime > 0)
		{
			//Vector3 v = (animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;
			Vector3 v = new Vector3(0,0,animator.GetFloat ("Speed"));
			// we preserve the existing y part of the current velocity.
			//v.y = m_Rigidbody.velocity.y;
			v = transform.InverseTransformDirection(v);
			v = new Vector3 (-v.x * runSpeedMultiplier, v.y * runSpeedMultiplier, v.z * runSpeedMultiplier);
			m_Rigidbody.velocity = v;
		}
	}
	void CheckGroundStatus()
	{
		RaycastHit hitInfo;
		#if UNITY_EDITOR
		// helper to visualise the ground check ray in the scene view
		Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
		#endif
		// 0.1f is a small offset to start the ray from inside the character
		// it is also good to note that the transform position in the sample assets is at the base of the character
		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
		{
			m_GroundNormal = hitInfo.normal;
			m_IsGrounded = true;
			animator.applyRootMotion = true;
		}
		else
		{
			m_IsGrounded = false;
			m_GroundNormal = Vector3.up;
			animator.applyRootMotion = false;
		}
	}
	void ApplyExtraTurnRotation()
	{
		// help the character turn faster (this is in addition to root rotation in the animation)
		float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
	}
	void UpdateAnimator(Vector3 move)
	{
		// update the animator parameters
		animator.SetFloat("Speed", m_ForwardAmount, 0.1f, Time.deltaTime);
		animator.SetFloat("Direction", m_TurnAmount, 0.1f, Time.deltaTime);
		//animator.SetBool("Jump", !m_IsGrounded);

		// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
		// which affects the movement speed because of the root motion.
		if (m_IsGrounded && move.magnitude > 0)
		{
			animator.speed = m_AnimSpeedMultiplier;
		}
		else
		{
			// don't use that while airborne
			animator.speed = 1;
		}
	}

	public void StickToWorldSpace(Transform root, Transform camera, ref float directionOut, ref float speedOut)
	{
		Vector3 rootDirection = root.forward;
		Vector3 stickDirection = new Vector3 (h, 0, v);
		speedOut = stickDirection.sqrMagnitude;

		// Get camera rotation
		Vector3 CameraDirection = camera.forward;
		CameraDirection.y = 0.0f;
		Quaternion referentialShift = Quaternion.FromToRotation (Vector3.forward, CameraDirection);

		//Convert joystick input in Worldspace coordinates
		Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross (moveDirection, rootDirection);

		Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
		Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.magenta);
		Debug.DrawRay (new Vector3 (root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.blue);

		float angleRootToMove = Vector3.Angle (rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);

		angleRootToMove /= 180f;

		directionOut = angleRootToMove * directionSpeed;
	}
}
