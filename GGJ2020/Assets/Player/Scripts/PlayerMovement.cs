using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Standard Movement")]
    public float acceleration = 1.0f;
    public float deceleration = 1.0f;
    public float maxSpeed = 200.0f;
    public float rotationSpeed;

    [Header("Dashing")]
    public float dashTime;
    public float dashSpeed;
    public bool stickyDashing;

    public Animator animator;

    private StateMachine<PlayerMovement> _stateMachine;
    public StateMachine<PlayerMovement> StateMachine {
        get { return _stateMachine; }
    }

    [SerializeField] private Vector3 _currentDirectionVector;
    public Vector3 CurrentDirectionVector {
        get { return _currentDirectionVector; }
        set { _currentDirectionVector = value; }
    }

    /// <returns>Returns true if directionVector is not a zero-vector</returns>
    public bool UpdateCurrentDirectionVector() {
        float x = Input.GetAxisRaw(InputStatics.HORIZONTAL);
        float z = Input.GetAxisRaw(InputStatics.VERTICAL);
        Vector3 directionVector = Vector3.Normalize(new Vector3(x, 0, z));
        if (directionVector != Vector3.zero) {
            _currentDirectionVector = directionVector;
            return true;
        }
        return false;
    }

    private float _currentSpeed;
    public float CurrentSpeed {
        get { return _currentSpeed; }
        set {
            if (value < maxSpeed * Time.deltaTime && value > 0.0f) {
                _currentSpeed = value;
            }
            else if (value >= maxSpeed * Time.deltaTime) {
                _currentSpeed = maxSpeed * Time.deltaTime;
            }
            else if (value <= 0.0f) {
                _currentSpeed = 0.0f;
            }
        }
    }

    public Rigidbody Rigidbody {
        get { return GetComponent<Rigidbody>(); }
    }


    private void Start() {
        _stateMachine = new StateMachine<PlayerMovement>(this);
        _stateMachine.ChangeState(new MovementState());
        _currentDirectionVector = new Vector3(0, 0, -1);
        transform.rotation = Quaternion.LookRotation(_currentDirectionVector);
    }

    void Update() {
        Repair();
        _stateMachine.Update();
    }

    private void Repair() {

    }
}
public class MovementState : State<PlayerMovement>
{
    private Timer _timer;
    public override void UpdateState(PlayerMovement owner) {
        Movement(owner);
        if (Input.GetKeyDown(KeyCode.Space)) {
            owner.StateMachine.ChangeState(new DashState());
        }
    }
    private void Movement(PlayerMovement owner) {
        owner.Rigidbody.velocity = Vector3.zero;
        if (owner.UpdateCurrentDirectionVector()) {
            owner.CurrentSpeed += owner.acceleration * Time.deltaTime;
            owner.animator.SetBool("running", true);
        }
        else {
            owner.CurrentSpeed -= owner.deceleration * Time.deltaTime;
            owner.animator.SetBool("running", false);
        }

        owner.animator.SetFloat("speed", owner.CurrentSpeed);
        owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, Quaternion.LookRotation(owner.CurrentDirectionVector), owner.rotationSpeed);
        owner.Rigidbody.velocity += owner.CurrentDirectionVector * owner.CurrentSpeed;
    }

    public override void EnterState(PlayerMovement owner) {
        owner.animator.SetBool("running", true);
    }
    public override void ExitState(PlayerMovement owner) {
        owner.animator.SetBool("running", false);
    }
}

public class DashState : State<PlayerMovement>
{
    private Timer _timer;
    public override void EnterState(PlayerMovement owner) {
        _timer = new Timer(owner.dashTime);
        if (owner.stickyDashing)
            owner.UpdateCurrentDirectionVector();
        owner.transform.rotation = Quaternion.LookRotation(owner.CurrentDirectionVector);
        owner.animator.SetBool("dashing", true);
        owner.animator.Play("anim_char_dash");
    }

    public override void UpdateState(PlayerMovement owner) { // TODO: Fucka andra när du dashar, exempelvis att du spawnar en collider/enablear
        _timer.Time += Time.deltaTime;
        if (_timer.Expired()) {
            owner.StateMachine.ChangeState(new MovementState());
        }
        else {
            if (!owner.stickyDashing) {
                float x = Input.GetAxisRaw(InputStatics.HORIZONTAL);
                float z = Input.GetAxisRaw(InputStatics.VERTICAL);
                Vector3 directionVector = Vector3.Normalize(new Vector3(x, 0, z));
                owner.CurrentDirectionVector += directionVector / 10;
                owner.CurrentDirectionVector = Vector3.Normalize(owner.CurrentDirectionVector);
                owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, Quaternion.LookRotation(owner.CurrentDirectionVector), owner.rotationSpeed);
            }
            owner.Rigidbody.velocity = owner.CurrentDirectionVector * owner.dashSpeed * Time.deltaTime;
        }
    }

    public override void ExitState(PlayerMovement owner) {
        owner.animator.SetBool("dashing", false);
        owner.animator.SetBool("maxRun", true);
        _timer.Reset();
    }
}