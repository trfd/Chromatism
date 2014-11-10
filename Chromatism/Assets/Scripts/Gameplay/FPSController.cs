using UnityEngine;
using System.Collections;

[AddComponentMenu("Gameplay/FPSController")]
public class FPSController : MonoBehaviour 
{

	#region Members

	// Constants
	public float _moveSpeed;
	public float _rotateSpeed;
	public float _jumpForce;
	public float _dashDistance;
	public float _dashSpeed;
	public float _dashCooldown;

	//reticle
	public GameObject _reticle;
	public GameObject _hitmarker;
	public float _reticlegrowth;

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
	private Timer m_dashTimer;
	private Vector3 m_dashDirection;
	private float m_dashDistanceRun = 0f;


	//reticle
	private float m_reticleSize = 0.01f;
	private Timer m_reticleTimer;
	private Timer m_hitmarkerTimer;
	private float m_growthCooldown;

	//Entity properties
	private EntityProperties m_properties;

	#endregion

	#region MonoBehaviour

	void Start ()
	{
		//move and camera controls
		KeyBinder.Instance.DefineActions("Forward", new KeyActionConfig(KeyType.Movement, 0, Forward, () => {m_forward -= 1;}));
		KeyBinder.Instance.DefineActions("Backward", new KeyActionConfig(KeyType.Movement, 1, Backward, () => {m_forward += 1;}));
		KeyBinder.Instance.DefineActions("StrafeLeft", new KeyActionConfig(KeyType.Movement, 2, StrafeLeft, () => {m_right += 1;}));
		KeyBinder.Instance.DefineActions("StrafeRight", new KeyActionConfig(KeyType.Movement, 3, StrafeRight, () => {m_right -= 1;}));
		KeyBinder.Instance.DefineActions("Jump/Dash", new KeyActionConfig(KeyType.Movement, 4, () => {if(m_canJump){rigidbody.AddForce(Vector3.up * m_properties.Gravity);m_canJump = false;}}, null));
		KeyBinder.Instance.DefineActions("MouseX", new AxisActionConfig(KeyType.Head, 0, MouseX));
		KeyBinder.Instance.DefineActions("MouseY", new AxisActionConfig(KeyType.Head, 0, MouseY));

		m_doubleKeyTimer = new Timer();
		m_dashTimer = new Timer();
		m_reticleTimer = new Timer();
		m_hitmarkerTimer = new Timer();

		m_properties = GetComponent<EntityProperties>();
		GPEventManager.Instance.Register("WeaponShoot", Shoot);
		GPEventManager.Instance.Register("EnemyTouched", OnTouch);
	}

	// applying movement
	void FixedUpdate ()
	{
		if(m_dashDirection != Vector3.zero)
		{
			transform.position += m_dashDirection * (m_properties.EntityVelocity * 4 * Time.deltaTime);
			m_dashDistanceRun += m_properties.EntityVelocity * 4 * Time.deltaTime;
			if(m_dashDistanceRun >= m_properties.EntityDashRange)
			{
				m_dashTimer.Reset(_dashCooldown);
				StopDash();
			}
			return;
		}

		if(m_forward != 0)
		{
			transform.position += transform.forward * (m_forward * m_properties.EntityVelocity * Time.deltaTime);
		}

		if(m_right != 0)
		{
			transform.position += transform.right * (m_right * m_properties.EntityVelocity * Time.deltaTime);
		}
	}

	void Update()
	{
		if(_reticle != null)
		{
			float size = Mathf.Lerp(m_reticleSize + _reticlegrowth, m_reticleSize, 1 - m_reticleTimer.CurrentNormalized);
			_reticle.transform.localScale = Vector3.one * size;
		}
		
		if(_hitmarker != null && !m_hitmarkerTimer.IsElapsedLoop)
		{
			float size = Mathf.Lerp(m_reticleSize + _reticlegrowth, m_reticleSize, m_hitmarkerTimer.CurrentNormalized);
			_hitmarker.transform.localScale = Vector3.one * size;
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
			StartDash(transform.forward);
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
			StartDash(-transform.forward);
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
			StartDash(-transform.right);
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
			StartDash(transform.right);
		}
		
		m_lastDirection = Direction.RIGHT;
		m_doubleKeyTimer.Reset(0.3f);
	}

	void StartDash(Vector3 direction)
	{
		if(m_dashTimer.IsElapsedLoop || _dashCooldown == 0)
		{
			m_dashDirection = direction;
			m_dashDistanceRun = 0f;
			rigidbody.velocity = Vector3.zero;
			rigidbody.useGravity = false;
		}
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
		transform.Rotate(Vector3.up * (value *_rotateSpeed));
	}

	//rotating camera up and down
	void MouseY(float value)
	{
		_camera.transform.Rotate(Vector3.right * (-value * _rotateSpeed));
	}


	void Shoot( string evtName, GPEvent gpEvent)
	{
		m_reticleTimer.Reset( (1/m_properties.WeaponFireRate) );
	} 

	void OnTouch( string evtName, GPEvent gpEvent)
	{
		m_hitmarkerTimer.Reset(0.1f);
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
