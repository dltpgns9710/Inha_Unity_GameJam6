using System;
using UnityEngine;

public class ObjectCollider : MonoBehaviour
{
    private bool _detect = false;
    public Action Broadcast;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _detect = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _detect = false;
    }
    public bool Detect()
    {
        return _detect;
    }
}
