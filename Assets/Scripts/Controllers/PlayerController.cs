using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 1. 위치 벡터
// 2. 방향 벡터
struct MyVector
{
    public float x;
    public float y;
    public float z;

    public float magnitude { get { return Mathf.Sqrt(x * x + y * y + z * z); } }

    public MyVector normalized { get { return new MyVector(x / magnitude, y / magnitude, z / magnitude); } }

    public MyVector(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }

    public static MyVector operator +(MyVector a, MyVector b)
    {
        return new MyVector(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static MyVector operator -(MyVector a, MyVector b)
    {
        return new MyVector(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static MyVector operator *(MyVector a, float b)
    {
        return new MyVector(a.x * b, a.y * b, a.z * b);
    }
}

public class PlayerController : BaseController
{
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    PlayerStat _stat;
    bool _stopSkill = false;

    

    public override void Init()
    {
        _stat = gameObject.GetComponent<PlayerStat>();
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateMoving()
    {
        // 몬스터가 내 사정거리보다 가까우면 공격
        if(_lockTarget != null)
		{
            float distance = (_destPos - transform.position).magnitude;
            if(distance <= 2)
			{
                State = Define.State.Skill;
                return;
			}
		}

        // 이동
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            Debug.DrawRay(transform.position, dir.normalized, Color.green);
            if(Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
			{
                if (Input.GetMouseButton(0) == false)
                    State = Define.State.Idle;
                return;
			}

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;

            //transform.position = transform.position + dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            //transform.LookAt(_destPos);
        }

       
    }

    protected override void UpdateSkill()
	{
        if(_lockTarget != null)
		{
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
		}
	}
	
    void OnHitEvent()
	{
        if(_lockTarget != null)
		{
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            PlayerStat myStat = gameObject.GetComponent<PlayerStat>();
            int damage = Mathf.Max(0, myStat.Attack - targetStat.Defense);
            targetStat.HP -= damage;
		}

        if(_stopSkill)
		{
            State = Define.State.Idle;
		}
        else
		{
            State = Define.State.Skill;
        }
    }

    //void OnKeyboard()
    //{
    //       if (Input.GetKey(KeyCode.W))
    //       {
    //           transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
    //           transform.position += Vector3.forward * Time.deltaTime * _speed;
    //       }
    //       if (Input.GetKey(KeyCode.S))
    //       {
    //           transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
    //           transform.position += Vector3.back * Time.deltaTime * _speed;
    //       }
    //       if (Input.GetKey(KeyCode.A))
    //       {
    //           transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
    //           transform.position += Vector3.left * Time.deltaTime * _speed;
    //       }
    //       if (Input.GetKey(KeyCode.D))
    //       {
    //           transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
    //           transform.position += Vector3.right * Time.deltaTime * _speed;
    //       }

    //       _moveToDest = false;
    //   }
    
    void OnMouseEvent(Define.MouseEvent evt)
    {
		//if (evt != Define.MouseEvent.Click)
		//    return;

		switch (State)
		{
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
				{
                    if(evt == Define.MouseEvent.PointerUp)
                        _stopSkill = true;
				}
                break;
        }
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
	{
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100f, _mask);
        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1.0f);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        _destPos = hit.point;
                        State = Define.State.Moving;
                        _stopSkill = false;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            _lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                    if (_lockTarget == null && raycastHit)
                        _destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }
    }
}
