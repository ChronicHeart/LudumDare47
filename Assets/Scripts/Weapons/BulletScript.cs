using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 5f;
    public int attackPower = 2;

    public float decayRate = 5f;

    public string alignment = "Player";

    private void Start()
    {
        Invoke("DestroySelf", decayRate);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (transform.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the other object is a bullet, destory this
        BulletScript otherBullet = other.GetComponent<BulletScript>();
        if(otherBullet != null)
        {
            if (otherBullet.alignment != alignment)
            {
                Destroy(other);
                DestroySelf();
                return;
            }
        }

        if(other.CompareTag(alignment) == false && other.isTrigger == false)
        {
            other.SendMessage("TakeDamage", attackPower, SendMessageOptions.DontRequireReceiver);
            CancelInvoke();
            DestroySelf();
        }
    }
}
