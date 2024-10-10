using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RushHourEffect : MonoBehaviour
{
    private ParticleSystem _rushHourEffect;
    private CookingManager _cookingManager;
    private float cameraTargetSize;
    private float targetCounter;
    private Camera _camera;


    // Start is called before the first frame update
    void Start()
    {
        _rushHourEffect = GetComponent<ParticleSystem>();
        _cookingManager = FindObjectOfType<CookingManager>();

        targetCounter = 0;
        cameraTargetSize = 5;

        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetCounter < 1)
        {
            targetCounter = Time.deltaTime;
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, cameraTargetSize, targetCounter);
        }

        //check if need to play
        //when rush hour and not playing
        if (_cookingManager.IsRushHour() && !_rushHourEffect.isPlaying)
        {
            _rushHourEffect.Play();
            targetCounter = 0;
            cameraTargetSize = 5.135f;

        }
        else if (!_cookingManager.IsRushHour() && _rushHourEffect.isPlaying)
        {
            _rushHourEffect.Stop();
            targetCounter = 0;
            cameraTargetSize = 5;
        }

    }
}
