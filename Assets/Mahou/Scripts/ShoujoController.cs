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
	public float speed = 0.0f;
	public float targetingSpeedMultiplier = 3f;
	public float direction = 0.0f;
	private float h = 0.0f;
	private float v = 0.0f;
	float m_TurnAmount;
	float m_ForwardAmount;
	private Transform m_Cam; 
	private Transform target;
	private Vector3 m_CamForward; 
	private Vector3 m_Move;
	Vector3 m_GroundNormal;
	public Text ljVText;
	public Text ljHText;
	public Text turnText;
	private ShoujoStates sState = ShoujoStates.Free;
	public enum ShoujoStates
	{
		Free,
		Target,
		Attack1,
		Attack2,
		Strafe
	}

	public Animator Animator
	{
		get
		{
			return this.animator;
		}
	}

	public float Speed
	{
		get
		{
			return this.speed;
		}
	}
	public float LocomotionThreshold { get { return 0.2f; } }
	void Start(){
		m_Cam = Camera.main.transform;
		animator = GetComponent<Animator>();
		m_Rigidbody = GetComponent<Rigidbody>();
		if (animator.layerCount >= 2) {
			animator.SetLayerWeight(1, 1);
		}
	}

	void Update(){
		if (Input.GetButtonDown("BButton")) {
			
		}
		if (Input.GetButtonDown("RBumper")) {
			animator.SetTrigger ("Attack1");
		} else {
			if (animator.GetBool ("Targeting")) {
				sState = ShoujoStates.Target;
			} else{
				sState = ShoujoStates.Free;
			}
		}
	}

	void FixedUpdate(){
		target = GameObject.FindWithTag ("Target").transform;
		if (animator) {
			h = Input.GetAxis("LJHorizontal");
			v = Input.GetAxis("LJVertical");
			ljHText.text = h.ToString ();
			ljVText.text = v.ToString ();
			//StickToWorldSpace (this.transform, m_Cam, ref directionDampTime, ref speed);
			/*animator.SetFloat("Speed", speed);
			animator.SetFloat("Direction", direction, directionDampTime, Time.deltaTime);*/
		}

		switch (sState) {
		case ShoujoStates.Free:
			if (m_Cam != null) {
				// calculate camera relative direction to move:
				m_CamForward = Vector3.Scale (m_Cam.forward, new Vector3 (1, 0, 1)).normalized;
				m_Move = v * m_CamForward + h * m_Cam.right;
			} else {
				// we use world-relative directions in the case of no main camera
				m_Move = v * Vector3.forward + h * Vector3.right;
			}

			if (m_Move.magnitude > 1f)
				m_Move.Normalize ();
			m_Move = transform.InverseTransformDirection (m_Move);
			CheckGroundStatus ();
			if (animator.GetBool("Controllable")) {
				m_Move = Vector3.ProjectOnPlane (m_Move, m_GroundNormal);
			} else {
				m_Move = Vector3.zero;
			}
			m_TurnAmount = Mathf.Atan2(m_Move.x, m_Move.z);

			turnText.text = m_TurnAmount.ToString();

			m_ForwardAmount = m_Move.z;
			if (m_ForwardAmount == 0) {
				m_TurnAmount = 0;
			}
			ApplyExtraTurnRotation();
			UpdateAnimator(m_Move);

			break;

		case ShoujoStates.Target:
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
			if (animator.GetBool("Controllable")) {
				m_Move = Vector3.ProjectOnPlane (m_Move, m_GroundNormal);
			} else {
				m_Move = Vector3.zero;
			}
			m_TurnAmount = h;

			turnText.text = m_TurnAmount.ToString();

			m_ForwardAmount = m_Move.z;
			if (m_ForwardAmount == 0) {
				m_TurnAmount = 0;
			}
			//ApplyExtraTurnRotation();
			transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));	
			UpdateAnimator(m_Move);
			break;
		}
	}
	void OnAnimatorMove() {
		// we implement this function to override the default root motion.
		// this allows us to modify the positional speed before it's applied.
		switch (sState) {
		case ShoujoStates.Free:
			//Vector3 v = (animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;
			Vector3 v = new Vector3(0,0,animator.GetFloat ("Speed"));
			// we preserve the existing y part of the current velocity.
			//v.y = m_Rigidbody.velocity.y;
			v = transform.InverseTransformDirection(v);
			v = new Vector3 (-v.x * runSpeedMultiplier, v.y * runSpeedMultiplier, v.z * runSpeedMultiplier);
			m_Rigidbody.velocity = v;
			break;
		case ShoujoStates.Target:
			Vector3 v1 = new Vector3(-animator.GetFloat ("Direction"),0,animator.GetFloat ("Speed"));
			v1 = transform.InverseTransformDirection(v1);
			v1 = new Vector3 (-v1.x * targetingSpeedMultiplier, v1.y * targetingSpeedMultiplier, v1.z * targetingSpeedMultiplier);
			m_Rigidbody.velocity = v1;
			break;
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

	void ResetAnimator()
	{
		animator.SetFloat ("Speed", 0);
		animator.SetFloat ("Direction", 0);
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
	public bool IsInLocomotion()
	{
		return animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Locomotion");
	}
	public bool IsInAttack()
	{
		return animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack1");
	}

	void Attack()
	{
		
	}
}
