using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
	// OnCollisionEnter 
	// 1. 나 혹은 상대한테 rigidbody가 있어야 한다(IsKinematic : off)
	// 2. 나한테 Collider가 있어야 한다(IsTrigger : off)
	// 3. 상대한테 Collider가 있어야 한다(IsTrigger : off)
	private void OnCollisionEnter(Collision collision)
	{
        Debug.Log($"Collision @ {collision.gameObject.name} !");
	}

	// OnTriggerEnter
	// 1. 둘 다 Collider가 있어야 한다
	// 2. 둘 중 하나는 IsTrigger : on
	// 3. 둘 줄 하나는 RigidBody가 있어야 한다
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log($"Trigger @ {other.gameObject.name} !");
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

		// local / world / viewport / screen

		// Debug.Log(Input.mousePosition); screen(픽셀 좌표)

		// Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition)); viewport(비율)

		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1.0f);
			RaycastHit hit;
			LayerMask mask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Wall");
			if(Physics.Raycast(ray, out hit, 100f))
			{
				Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name} !");
			}
		}

		//if (Input.GetMouseButtonDown(0))
		//{
		//	Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
		//	Vector3 dir = mousePos - Camera.main.transform.position;
		//	dir = dir.normalized;

		//	Debug.DrawRay(Camera.main.transform.position, dir * 100f, Color.red, 1.0f);
		//	RaycastHit hit;

		//	if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 100f))
		//	{
		//		Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name} !");
		//	}
		//}
	}
}
