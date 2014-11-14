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
	public float _reticlegrowth;
	public float _reticleSize = 0.01f;

	// Refs
	public GameObject _camera;
	public Animator _weaponAnimator;


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
	private Timer m_reticleTimer;
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
		KeyBinder.Instance.DefineActions("Jump/Dash", new KeyActionConfig(KeyType.Movement, 4, () => {if(m_canJump){Jump();}}, null));
		KeyBinder.Instance.DefineActions("MouseX", new AxisActionConfig(KeyType.Head, 0, MouseX));
		KeyBinder.Instance.DefineActions("MouseY", new AxisActionConfig(KeyType.Head, 0, MouseY));

		m_doubleKeyTimer = new Timer();
		m_dashTimer = new Timer();
		m_reticleTimer = new Timer();

		m_properties = GetComponent<EntityProperties>();
		GPEventManager.Instance.Register("PlayerWeaponShoot", Shoot);
		GPEventManager.Instance.Register("PlayerWeaponStartReload", StartReload);
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
			float size = Mathf.Lerp(_reticleSize + _reticlegrowth, _reticleSize, 1 - m_reticleTimer.CurrentNormalized);
			_reticle.transform.localScale = Vector3.one * size;
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

	void Jump()
	{
		rigidbody.AddForce(Vector3.up * m_properties.Gravity);
		m_canJump = false;
		
		Fabric.EventManager.Instance.PostEvent("foot_jump",gameObject);
		Fabric.EventManager.Instance.PostEvent("foot_on",gameObject);
	}

	void Grounding()
	{
		m_canJump = true;
		
		Fabric.EventManager.Instance.PostEvent("foot_fall",gameObject);
		Fabric.EventManager.Instance.PostEvent("foot_on",gameObject);
	}

	#endregion

	#region Camera

	// rotating camera left and right
	void MouseX(float value)
	{
		transform.Rotate(Vector3.up, (value *_rotateSpeed));
	}

	//rotating camera up and down
	void MouseY(float value)
	{
		Vector3 currEuler = _camera.transform.localRotation.eulerAngles;
		currEuler.x += (-value * _rotateSpeed);

		if( currEuler.x > 90 && currEuler.x < 180 )
		{
			currEuler.x = 90f;
		}

		if( currEuler.x > 180 && currEuler.x < 270)
		{
			currEuler.x = 270;
		}
			_camera.transform.localRotation = Quaternion.Euler(currEuler);		
		//_camera.transform.Rotate(Vector3.right, (-value * _rotateSpeed));
	}


	void Shoot( string evtName, GPEvent gpEvent)
	{
		m_reticleTimer.Reset( (1/m_properties.WeaponFireRate) );
		_weaponAnimator.SetTrigger("Shoot");
	} 
	
	#endregion

	#region Collision

	// verifying if we can jump again
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag ("ground"))
			Grounding ();

		StopDash();
		rigidbody.velocity = Vector3.zero;
		
	}

	#endregion

	void StartReload(string evtName, GPEvent gpEvent)
	{
		_weaponAnimator.SetTrigger("Reload");
	}
}
