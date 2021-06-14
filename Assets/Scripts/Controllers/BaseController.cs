using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Vector3 _destPos;

    [SerializeField]
    protected float _yAngle = 0f;

    [SerializeField]
    protected Define.State _state = Define.State.Idle;

    protected GameObject _lockTarget;

    protected Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;

            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case Define.State.Die:
                    break;
                case Define.State.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case Define.State.Moving:
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case Define.State.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;
            }
        }
    }

	private void Start()
	{
        Init();
	}

	

    void Update()
    {
        switch (State)
        {
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;
        }


        #region 노트
        //_yAngle += Time.deltaTime * 100f;

        // 절대 회전값
        // transform.eulerAngles = new Vector3(0f, _yAngle, 0f);

        // += delta(incremental value)
        //transform.Rotate(new Vector3(0f, Time.deltaTime * 100f, 0f));

        //transform.rotation = Quaternion.Euler(new Vector3(0f, _yAngle, 0f));


        // local -> world
        // transform.TransformDirection()

        // world -> local
        //transform.InverseTransformDirection()

        //if(Input.GetKey(KeyCode.W))
        //    transform.position += transform.TransformDirection(Vector3.forward * Time.deltaTime * _speed);
        //if (Input.GetKey(KeyCode.S))
        //    transform.position += transform.TransformDirection(Vector3.back * Time.deltaTime * _speed);
        //if (Input.GetKey(KeyCode.A))
        //    transform.position += transform.TransformDirection(Vector3.left * Time.deltaTime * _speed);
        //if (Input.GetKey(KeyCode.D))
        //    transform.position += transform.TransformDirection(Vector3.right * Time.deltaTime * _speed);

        //if (Input.GetKey(KeyCode.W))
        //    transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        //if (Input.GetKey(KeyCode.S))
        //    transform.Translate(Vector3.back * Time.deltaTime * _speed);
        //if (Input.GetKey(KeyCode.A))
        //    transform.Translate(Vector3.left * Time.deltaTime * _speed);
        //if (Input.GetKey(KeyCode.D))
        //    transform.Translate(Vector3.right * Time.deltaTime * _speed);

        //if (Input.GetKey(KeyCode.W))
        //    transform.rotation = Quaternion.LookRotation(Vector3.forward);
        //if (Input.GetKey(KeyCode.S))
        //    transform.rotation = Quaternion.LookRotation(Vector3.back);
        //if (Input.GetKey(KeyCode.A))
        //    transform.rotation = Quaternion.LookRotation(Vector3.left);
        //if (Input.GetKey(KeyCode.D))
        //    transform.rotation = Quaternion.LookRotation(Vector3.right);
        #endregion
    }

    public abstract void Init();
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateDie() { }
    protected virtual void UpdateSkill() { }
}
