using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public Define.CameraMode _mode = Define.CameraMode.QuarterView;

    [SerializeField]
    public Vector3 _delta = new Vector3(0f, 6f, -5f);
    
    [SerializeField]
    private GameObject _player = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuarterView)
        {
            if(_player == null)
			{
                return;
			}

            RaycastHit hit;
            if(Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
			{
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist;
			}
            else
			{
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform.position);
            }
        }
    }

    public void SetQuarterView(Vector3 delta)
	{
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
	}
}
