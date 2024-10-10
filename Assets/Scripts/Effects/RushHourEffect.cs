using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushHourEffect : MonoBehaviour
{
    private ParticleSystem _rushHourEffect;
    private CookingManager _cookingManager;

    // Start is called before the first frame update
    void Start()
    {
        _rushHourEffect = GetComponent<ParticleSystem>();
        _cookingManager = FindObjectOfType<CookingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //check if need to play
        //when rush hour and not playing
        if (_cookingManager.IsRushHour() && !_rushHourEffect.isPlaying)
        {
            _rushHourEffect.Play();
        }
        else if (!_cookingManager.IsRushHour() && _rushHourEffect.isPlaying)
        {
            _rushHourEffect.Stop();
        }

    }
}
