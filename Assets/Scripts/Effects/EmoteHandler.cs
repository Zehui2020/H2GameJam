using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteHandler : MonoBehaviour
{
    Vector3 speed;
    float timer;

    // Start is called before the first frame update
    public void Init(Vector3 startSpeed)
    {
        speed = startSpeed;

        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        speed.x += (0 - speed.x) / 800;
        speed.y += 0.01f;

        transform.position += speed * Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= 5)
        {
            Destroy(gameObject);
        }
    }
}
