using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class StepOnDanger : MonoBehaviour
{
    [SerializeField] CinemachineBasicMultiChannelPerlin _shake;
    [SerializeField] AnimationCurve _shakeCurve;
    [SerializeField] float _shakeTime;
    [SerializeField] float _shakeGain = 4f;
    [SerializeField] Rigidbody2D _myRB;
    [SerializeField] Movement _myMovement;
    [SerializeField] Animator _myAnimator;

    float _shakingTime = 0;
    Vector2 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        if (_shakingTime > 0)
        {
            _shake.AmplitudeGain = _shakeCurve.Evaluate(1f - (_shakingTime / _shakeTime)) * _shakeGain;
            _shakingTime -= Time.deltaTime;

            if (_shakingTime <= _shakeTime / 2f)
            {
                gameObject.transform.position = _startPosition;
            }

            if (_shakingTime <= 0)
            {
                _myRB.simulated = true;
                _myMovement.enabled = true;
            }
        }
        else if (_shake.AmplitudeGain > 0)
        {
            _shake.AmplitudeGain = 0;
        }
    }

    public void StepOnDangerEvent(GameObject targetObject)
    {
        _myAnimator.SetTrigger("Respawn");
        _shakingTime = _shakeTime;
        _myRB.simulated = false;
        _myMovement.enabled = false;
    }
}
