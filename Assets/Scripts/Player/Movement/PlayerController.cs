using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Coroutine _fakeGrafity;

    [SerializeField] private LayerMask _LayerMask;
    private RaycastHit _hit;

    private Transform _jumpRayPosition;
    private Transform _topRayPos;

    private int xDir;
    private int zDir;

    [SerializeField] private float _MovementSpeed;
    [SerializeField] private float _JumpPower;
    [SerializeField] private float _Grafity;
    [SerializeField] private float _MaxDownForce;
    [SerializeField] private float _AccelirationSpeed;
    [SerializeField] private float _DeccelirationSpeed;

    private Vector3 _velocity;
    private Vector3 _savedSpeed;
    private Vector3 _acseliration;
    private Vector3 _tempAcseliration;

    private bool _decelerationX;
    private bool _decelerationZ;
    private bool xClamped;
    private bool zClamped;
    public bool _GamePaused;
    public bool _hitWall;

    private void Start()
    {
        _jumpRayPosition = GameObject.FindGameObjectWithTag("JumpRayPoint").transform;
        _topRayPos = GameObject.FindGameObjectWithTag("TopRayPos").transform;
    }

    void Update()
    {
        Movement();
        if (_hitWall == false)
        {
            transform.Translate(_velocity);
        }
        else
        {
            transform.Translate(-_velocity);
        }

        JumpingAndFalling();
    }

    private void Movement()
    {
        //movement
        if (_decelerationX == false && Input.GetAxisRaw("Horizontal") == 0 && _savedSpeed.x != 0)
        {
            StartCoroutine(Decelaration(true, false));
        }

        if (_decelerationZ == false && Input.GetAxisRaw("Vertical") == 0 && _savedSpeed.z != 0)
        {
            StartCoroutine(Decelaration(false, true));
        }

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (_acseliration.x < 0.95)
            {
                _acseliration.x += (1 - _acseliration.x) * Time.unscaledDeltaTime * _AccelirationSpeed;
            }
            else
            {
                _acseliration.x = 1;
            }
            xDir = (int)Input.GetAxisRaw("Horizontal");
        }
        _velocity.x = Mathf.Lerp(0, _MovementSpeed * xDir, _acseliration.x);

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            if (_acseliration.z < 0.95)
            {
                _acseliration.z += (1 - _acseliration.z) * Time.unscaledDeltaTime * _AccelirationSpeed;
            }
            else
            {
                _acseliration.z = 1;
            }
            zDir = (int)Input.GetAxisRaw("Vertical");
        }
        _velocity.z = Mathf.Lerp(0, _MovementSpeed * zDir, _acseliration.z);

        if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") != 0)
        {
            if (xClamped == true)
            {
                _acseliration.x = Mathf.Clamp(_acseliration.x, 0, 0.5f);
            }

            if (zClamped == true)
            {
                _acseliration.z = Mathf.Clamp(_acseliration.x, 0, 0.5f);
            }

            if (_acseliration.x <= 0.5)
            {
                xClamped = true;
            }
            else
            {
                _tempAcseliration.x = _acseliration.x - _acseliration.z;
                _velocity.x = Mathf.Lerp(0, _MovementSpeed * xDir, _tempAcseliration.x);
            }

            if (_acseliration.z <= 0.5)
            {
                zClamped = true;
            }
            else
            {
                _tempAcseliration.z = _acseliration.z - _acseliration.x;
                _velocity.z = Mathf.Lerp(0, _MovementSpeed * zDir, _tempAcseliration.z);
            }
        }
        else
        {
            if (_tempAcseliration.x != 0)
            {
                _acseliration.x = _tempAcseliration.x;
            }

            if (_tempAcseliration.z != 0)
            {
                _acseliration.z = _tempAcseliration.z;
            }
            _tempAcseliration = Vector3.zero;
            xClamped = false;
            zClamped = false;
        }

        _velocity *= Time.unscaledDeltaTime;

        _savedSpeed = _velocity;
    }

    private IEnumerator Decelaration(bool xDeceliration, bool zDeceliration)
    {
        if (xDeceliration == true)
        {
            while (_acseliration.x > 0)
            {
                if (_acseliration.x > 0.1)
                {
                    _acseliration.x -= _acseliration.x * Time.unscaledDeltaTime * _DeccelirationSpeed;
                }
                else
                {
                    _acseliration.x = 0;
                }

                if (xDeceliration == true)
                {
                    if (Input.GetAxisRaw("Horizontal") != 0)
                    {
                        _decelerationX = false;
                        break;
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }

        if (zDeceliration == true)
        {
            while (_acseliration.z > 0)
            {
                if (_acseliration.z > 0.1)
                {
                    _acseliration.z -= _acseliration.z * Time.unscaledDeltaTime * _DeccelirationSpeed;
                }
                else
                {
                    _acseliration.z = 0;
                }

                if (zDeceliration == true)
                {
                    if (Input.GetAxisRaw("Vertical") != 0)
                    {
                        _decelerationZ = false;
                        break;
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }

        if (xDeceliration == true)
        {
            _decelerationX = false;
        }

        if (zDeceliration == true)
        {
            _decelerationZ = false;
        }
    }

    private void JumpingAndFalling()
    {
        //raycast that sees if your on the ground or not
        Physics.Raycast(_jumpRayPosition.position + new Vector3(0, transform.lossyScale.y / 2 - (transform.lossyScale.y / 2) / 10, 0), Vector3.down, out _hit, Mathf.Infinity, _LayerMask);
        if ((_hit.transform == null || _hit.distance - (transform.lossyScale.y / 2 - (transform.lossyScale.y / 2) / 10) > 0.1) && _fakeGrafity == null)
        {
            _fakeGrafity = StartCoroutine(FakeGrafity(0));
        }
        else if (_hit.transform != null && Input.GetKeyDown(KeyCode.Space) && _fakeGrafity == null)
        {
            _fakeGrafity = StartCoroutine(FakeGrafity(_JumpPower));
            StartCoroutine(CeilingCollision());
        }
        else if (_fakeGrafity == null)
        {
            transform.position = new Vector3(transform.position.x, _hit.point.y + (transform.position.y - _jumpRayPosition.position.y) + 0.03f, transform.position.z);
        }
    }

    private IEnumerator FakeGrafity(float leftoverForce)
    {
        while ((((float)(int)(_hit.point.y * 100)) / 100) <= _jumpRayPosition.position.y || _hit.transform == null)
        {
            transform.Translate(0, leftoverForce, 0);
            if (leftoverForce <= 0)
            {
                leftoverForce += (-_MaxDownForce - leftoverForce) * _Grafity * 0.01f;
            }
            else
            {
                leftoverForce -= _Grafity * 0.01f;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }

        transform.Translate(0, _hit.point.y - _jumpRayPosition.position.y + 0.03f, 0);

        _fakeGrafity = null;
    }

    private IEnumerator CeilingCollision()
    {
        RaycastHit hit;
        while (_fakeGrafity != null)
        {
            yield return new WaitForEndOfFrame();
            Physics.Raycast(_topRayPos.position - new Vector3(0, transform.lossyScale.y / 2, 0), -Vector3.down, out hit, Mathf.Infinity, _LayerMask);
            if (hit.transform != null && hit.distance - transform.lossyScale.y / 2 < 0.03)
            {
                StopCoroutine(_fakeGrafity);
                _fakeGrafity = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _hitWall = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _hitWall = false;
    }
}