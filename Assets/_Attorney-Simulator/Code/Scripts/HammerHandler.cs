using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HammerHandler : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] string velocityMin;
    [SerializeField] UnityEvent whenHitBase;

    void Start()
    {
        // Get the Rigidbody component attached to the GameObject
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("HammerBase"))
        {
            Debug.Log(rb.velocity.magnitude);
            float.TryParse(velocityMin, out float floatValue);
            if (rb.velocity.magnitude < floatValue) return;
            whenHitBase.Invoke();
        }
    }
}
