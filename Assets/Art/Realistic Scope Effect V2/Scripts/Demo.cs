using System;
using UnityEngine;


    public class Demo : MonoBehaviour
    {
		public Vector2 rotationRange = new Vector3(70f, 70f);

		public float rotationSpeed = 10f;

		public float dampingTime = 0.2f;

		public bool relative = true;

		private Vector3 m_TargetAngles;

		private Vector3 m_FollowAngles;

		private Vector3 m_FollowVelocity;

		private Quaternion m_OriginalRotation;

	public GameObject Scope;
	public GameObject Reflex;

		private void Start()
		{
			m_OriginalRotation = base.transform.localRotation;
		}

	public void quitApplication()
	{
		Application.Quit();
	}

		public void Update()
		{

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Reflex.SetActive(false);
			Scope.SetActive(true);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			Reflex.SetActive(true);
			Scope.SetActive(false);
		}

		base.transform.localRotation = m_OriginalRotation;
			if (relative)
			{
				float axis = Input.GetAxis("Mouse X");
				float axis2 = Input.GetAxis("Mouse Y");
				if (m_TargetAngles.y > 180f)
				{
					m_TargetAngles.y -= 360f;
					m_FollowAngles.y -= 360f;
				}
				if (m_TargetAngles.x > 180f)
				{
					m_TargetAngles.x -= 360f;
					m_FollowAngles.x -= 360f;
				}
				if (m_TargetAngles.y < -180f)
				{
					m_TargetAngles.y += 360f;
					m_FollowAngles.y += 360f;
				}
				if (m_TargetAngles.x < -180f)
				{
					m_TargetAngles.x += 360f;
					m_FollowAngles.x += 360f;
				}
				m_TargetAngles.y += axis * rotationSpeed;
				m_TargetAngles.x += axis2 * rotationSpeed;
				m_TargetAngles.y = Mathf.Clamp(m_TargetAngles.y, (0f - rotationRange.y) * 0.5f, rotationRange.y * 0.5f);
				m_TargetAngles.x = Mathf.Clamp(m_TargetAngles.x, (0f - rotationRange.x) * 0.5f, rotationRange.x * 0.5f);
			}
			else
			{
				float axis = Input.mousePosition.x;
				float axis2 = Input.mousePosition.y;
				m_TargetAngles.y = Mathf.Lerp((0f - rotationRange.y) * 0.5f, rotationRange.y * 0.5f, axis / (float)Screen.width);
				m_TargetAngles.x = Mathf.Lerp((0f - rotationRange.x) * 0.5f, rotationRange.x * 0.5f, axis2 / (float)Screen.height);
			}
			m_FollowAngles = Vector3.SmoothDamp(m_FollowAngles, m_TargetAngles, ref m_FollowVelocity, dampingTime);
			base.transform.localRotation = m_OriginalRotation * Quaternion.Euler(0f - m_FollowAngles.x, m_FollowAngles.y, 0f);
		}
	}

