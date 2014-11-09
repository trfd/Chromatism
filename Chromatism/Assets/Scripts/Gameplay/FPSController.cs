using UnityEngine;
using System.Collections;

public class FPSController : MonoBehaviour {

	// Constants
	public float _moveSpeed;
	public float _rotateSpeed;
	public float _jumpForce;

	//Refs
	public GameObject _camera;

	private int m_forward;
	private int m_right;
	private bool m_ableToJump = true;

	void Start ()
	{
		KeyBinder.Instance.DefineActions("Forward", new KeyActionConfig(KeyType.Movement, 0, () => {if(m_forward <1)m_forward += 1;}, () => {m_forward -= 1;}));
		KeyBinder.Instance.DefineActions("Backward", new KeyActionConfig(KeyType.Movement, 1, () => {if(m_forward > -1)m_forward -= 1;}, () => {m_forward += 1;}));
		KeyBinder.Instance.DefineActions("StrafeLeft", new KeyActionConfig(KeyType.Movement, 2, () => {if(m_right > -1)m_right -= 1;}, () => {m_right += 1;}));
		KeyBinder.Instance.DefineActions("StrafeRight", new KeyActionConfig(KeyType.Movement, 3, () => {if(m_right < 1)m_right += 1;}, () => {m_right -= 1;}));
		KeyBinder.Instance.DefineActions("Jump", new KeyActionConfig(KeyType.Movement, 4, () => {if(m_ableToJump){rigidbody.AddForce(Vector3.up * _jumpForce);m_ableToJump = false;}}, null));
		KeyBinder.Instance.DefineActions("MouseX", new AxisActionConfig(KeyType.Head, 0, MouseX));
		KeyBinder.Instance.DefineActions("MouseY", new AxisActionConfig(KeyType.Head, 0, MouseY));
	}
	
	void FixedUpdate ()
	{
		if(m_forward != 0)
		{
			transform.position += transform.forward * (m_forward * _moveSpeed * Time.deltaTime);
		}

		if(m_right != 0)
		{
			transform.position += transform.right * (m_right * _moveSpeed * Time.deltaTime);
		}
	}
	
	void MouseX(float value)
	{
		transform.Rotate(Vector3.up * (value *_rotateSpeed * Time.deltaTime));
	}
	
	void MouseY(float value)
	{
		_camera.transform.Rotate(Vector3.right * (-value * _rotateSpeed * Time.deltaTime));
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("ground"))
			m_ableToJump = true;
		
	}
}
