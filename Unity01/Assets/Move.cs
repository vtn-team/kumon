using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidBody;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO
        //Unity-1: 移動せよ
        //https://candle-stoplight-544.notion.site/Unity-1-4e021f226d584730b715626436ccc330


    }
}
