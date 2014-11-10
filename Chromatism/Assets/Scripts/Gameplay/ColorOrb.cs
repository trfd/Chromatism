//
// ColorOrb.cs
//
// Author(s):
//       Baptiste Dupy <baptiste.dupy@gmail.com>
//
// Copyright (c) 2014
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using UnityEngine;
using System.Collections;

public class ColorOrb : MonoBehaviour
{
	#region Private Members

	public PlayerBehaviour m_player;

	public Vector3 m_random;

	public bool m_isMovingToPlayer;

	#endregion

	#region Public Members

	public float _playerSnapDistance;

	public float _velocity;

	public Channel _channel;

	#endregion

	#region Properties

	public float ColorValue
	{
		get; set;
	}

	public Vector3 StationnaryLocation
	{
		get; set;
	}

	#endregion

	#region Constructor

	public ColorOrb()
	{

	}

	#endregion

	#region MonoBehaviour

	void Start()
	{
		m_random = new Vector3(Random.Range(-4f,4f),Random.Range(-4f,4f),Random.Range(-4f,4f));

		m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
	}

	void Update()
	{
		if(Vector3.Distance(m_player.transform.position,transform.position) < _playerSnapDistance )
		   m_isMovingToPlayer = true;

		if(m_isMovingToPlayer)
		{
			transform.position += (m_player.transform.position - transform.position) * Time.deltaTime * 4f * _velocity;
		}
		else
		{
			Vector3 offset = new Vector3(0.1f * Mathf.Sin(Time.timeSinceLevelLoad * m_random.x), 0,
		        	                     0.1f * Mathf.Cos(Time.timeSinceLevelLoad * m_random.z));
			transform.position += (StationnaryLocation - transform.position + offset) * 
				Time.deltaTime * _velocity;
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		TreatCollision(other);
	}

	void OnTriggerStay(Collider other) 
	{
		TreatCollision(other);
	}

	#endregion

	void TreatCollision(Collider other)
	{
		if(other.tag != "Player")
			return;
		
		m_player.PickUpOrb(this);
		
		Destroy(this.gameObject);
	}
}
