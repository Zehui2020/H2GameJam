using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerEmoter : MonoBehaviour
{
    [SerializeField] private GameObject EmoteHandlerPrefab;
    float timer;

    private void Start()
    {
        timer = 5;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 5)
        {
            timer = 0;

            //spawn emotes
            int noEmotes = Random.Range(4, 7);
            for (int i = 0; i < noEmotes; i++)
            {
                GameObject newEffect = Instantiate(EmoteHandlerPrefab);
                newEffect.transform.position = transform.position;

                newEffect.GetComponent<EmoteHandler>().Init(new Vector3( Random.Range(-1.0f, 1.0f), Random.Range(-0.5f, -1.0f), 0).normalized * 7);
            }
        }
    }
}
