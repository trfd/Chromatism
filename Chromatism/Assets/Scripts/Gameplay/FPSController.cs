using UnityEngine;
using System.Collections;

[AddComponentMenu("Gameplay/FPSController")]
public class FPSController : MonoBehaviour {

	#region Members

	// Constants
	public float _moveSpeed;
	public float _rotateSpeed;
	public float _jumpForce;
	public float _dashDistance;
	public float _dashSpeed;

	// Refs
	public GameObject _camera;
	public Pool _pool;


	// Movement members
	private int m_forward = 0;
	private int m_right = 0;
	private bool m_canJump = false;

	enum Direction
	{
		NONE,
		FORWARD,
		BACKWARD,
		LEFT,
		RIGHT
	}

	// Dash
	private Direction m_lastDirection; 
	private Timer m_doubleKeyTimer;

	private Vector3 m_dashDirection;
	private float m_dashDistanceRun = 0f;


	#endregion

	#region MonoBehaviour

	void Start ()
	{
		//move and camera controls
		KeyBinder.Instance.DefineActions("Forward", new KeyActionConfig(KeyType.Movement, 0, Forward, () => {m_forward -= 1;}));
		KeyBinder.Instance.DefineActions("Backward", new KeyActionConfig(KeyType.Movement, 1, Backward, () => {m_forward += 1;}));
		KeyBinder.Instance.DefineActions("StrafeLeft", new KeyActionConfig(KeyType.Movement, 2, StrafeLeft, () => {m_right += 1;}));
		KeyBinder.Instance.DefineActions("StrafeRight", new KeyActionConfig(KeyType.Movement, 3, StrafeRight, () => {m_right -= 1;}));
		KeyBinder.Instance.DefineActions("Jump/Dash", new KeyActionConfig(KeyType.Movement, 4, () => {if(m_canJump){rigidbody.AddForce(Vector3.up * _jumpForce);m_canJump = false;}}, null));
		KeyBinder.Instance.DefineActions("MouseX", new AxisActionConfig(KeyType.Head, 0, MouseX));
		KeyBinder.Instance.DefineActions("MouseY", new AxisActionConfig(KeyType.Head, 0, MouseY));

		m_doubleKeyTimer = new Timer();

	}

	// applying movement
	void FixedUpdate ()
	{
		if(m_dashDirection != null && m_dashDirection != Vector3.zero)
		{
			transform.position += m_dashDirection * (_dashSpeed * Time.deltaTime);
			m_dashDistanceRun += _dashSpeed * Time.deltaTime;
			if(m_dashDistanceRun >= _dashDistance)
			{
				StopDash();
			}
			return;
		}

		if(m_forward != 0)
		{
			transform.position += transform.forward * (m_forward * _moveSpeed * Time.deltaTime);
		}

		if(m_right != 0)
		{
			transform.position += transform.right * (m_right * _moveSpeed * Time.deltaTime);
		}
	}

	#endregion


	#region Movement

	void Forward()
	{
		if(m_forward <1)
			m_forward += 1;

		if(m_lastDirection == Direction.FORWARD && !m_doubleKeyTimer.IsElapsedLoop && m_dashDirection == Vector3.zero && !m_canJump)
		{
			m_dashDirection = transform.forward;
			m_dashDistanceRun = 0f;
			rigidbody.useGravity = false;
		}

		m_lastDirection = Direction.FORWARD;
		m_doubleKeyTimer.Reset(0.3f);
	}

	void Backward()
	{
		if(m_forward > -1)
			m_forward -= 1;

		if(m_lastDirection == Direction.BACKWARD && !m_doubleKeyTimer.IsElapsedLoop && m_dashDirection == Vector3.zero && !m_canJump)
		{
			m_dashDirection = -transform.forward;
			m_dashDistanceRun = 0f;
			rigidbody.useGravity = false;
		}
		
		m_lastDirection = Direction.BACKWARD;
		m_doubleKeyTimer.Reset(0.3f);
	}

	void StrafeLeft()
	{
		if(m_right > -1)
			m_right -= 1;

		if(m_lastDirection == Direction.LEFT && !m_doubleKeyTimer.IsElapsedLoop && m_dashDirection == Vector3.zero && !m_canJump)
		{
			m_dashDirection = -transform.right;
			m_dashDistanceRun = 0f;
			rigidbody.useGravity = false;
		}
		
		m_lastDirection = Direction.LEFT;
		m_doubleKeyTimer.Reset(0.3f);
	}

	void StrafeRight()
	{
		if(m_right < 1)
			m_right += 1;

		if(m_lastDirection == Direction.RIGHT && !m_doubleKeyTimer.IsElapsedLoop && m_dashDirection == Vector3.zero && !m_canJump)
		{
			m_dashDirection = transform.right;
			m_dashDistanceRun = 0f;
			rigidbody.useGravity = false;
		}
		
		m_lastDirection = Direction.RIGHT;
		m_doubleKeyTimer.Reset(0.3f);
	}

	void StopDash()
	{
		rigidbody.useGravity = true;
		m_dashDirection = Vector3.zero;
	}

	#endregion

	#region Camera

	// rotating camera left and right
	void MouseX(float value)
	{
		transform.Rotate(Vector3.up * (value *_rotateSpeed * Time.deltaTime));
	}

	//rotating camera up and down
	void MouseY(float value)
	{
		_camera.transform.Rotate(Vector3.right * (-value * _rotateSpeed * Time.deltaTime));
	}

	#endregion

	#region Collision

	// verifying if we can jump again
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("ground"))
			m_canJump = true;

		StopDash();
		
	}

	#endregion
}
