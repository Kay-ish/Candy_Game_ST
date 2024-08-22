using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    Vector3 startPosition;

    public float amplitude;
    public float speed;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {

        //行ったり来たりするため
        // Sinに時間関数をかけるパターン
        float z = amplitude * Mathf.Sin(Time.time * speed);

        // zを変位させたポジションに再設定
        transform.localPosition = startPosition + new Vector3(0, 0, z);
    }
}
