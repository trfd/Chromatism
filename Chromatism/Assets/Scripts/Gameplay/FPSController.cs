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
	private int m_jumpDash = 0;

	private Vector3 m_dashDirection;
	private float m_dashDistanceRun = 0f;


	#endregion

	#region MonoBehaviour

	void Start ()
	{
		//move and camera controls
		KeyBinder.Instance.DefineActions("Forward", new KeyActionConfig(KeyType.Movement, 0, () => {if(m_forward <1)m_forward += 1;}, () => {m_forward -= 1;}));
		KeyBinder.Instance.DefineActions("Backward", new KeyActionConfig(KeyType.Movement, 1, () => {if(m_forward > -1)m_forward -= 1;}, () => {m_forward += 1;}));
		KeyBinder.Instance.DefineActions("StrafeLeft", new KeyActionConfig(KeyType.Movement, 2, () => {if(m_right > -1)m_right -= 1;}, () => {m_right += 1;}));
		KeyBinder.Instance.DefineActions("StrafeRight", new KeyActionConfig(KeyType.Movement, 3, () => {if(m_right < 1)m_right += 1;}, () => {m_right -= 1;}));
		KeyBinder.Instance.DefineActions("Jump/Dash", new KeyActionConfig(KeyType.Movement, 4, JumpOrDash, null));
		KeyBinder.Instance.DefineActions("MouseX", new AxisActionConfig(KeyType.Head, 0, MouseX));
		KeyBinder.Instance.DefineActions("MouseY", new AxisActionConfig(KeyType.Head, 0, MouseY));

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

	void JumpOrDash()
	{
		switch(m_jumpDash)
		{
			// Jump
			case 0 : 
				rigidbody.AddForce(Vector3.up * _jumpForce);
				m_jumpDash ++;
				break;
			// Dash
			case 1 :
				if(m_right == 0)
					m_dashDirection = _camera.transform.forward;
				else
					m_dashDirection = transform.right * m_right;

				m_dashDistanceRun = 0f;
				rigidbody.useGravity = false;
				m_jumpDash ++;
				break;
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
			m_jumpDash = 0;

		StopDash();
		
	}

	#endregion
}
