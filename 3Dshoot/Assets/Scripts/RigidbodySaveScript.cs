using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySaveScript : MonoBehaviour, ISaveable
{
    public object captureState()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        RigidbodySaveData rbsd = new RigidbodySaveData();

        rbsd.isEnabled = gameObject.activeSelf;

        rbsd.position = new float[3];
        rbsd.position[0] = transform.position.x;
        rbsd.position[1] = transform.position.y;
        rbsd.position[2] = transform.position.z;

        rbsd.rotation = new float[4];
        rbsd.rotation[0] = transform.rotation.x;
        rbsd.rotation[1] = transform.rotation.y;
        rbsd.rotation[2] = transform.rotation.z;
        rbsd.rotation[3] = transform.rotation.w;

        if (rb)
        {
            rbsd.velocity = new float[3];
            rbsd.velocity[0] = rb.velocity.x;
            rbsd.velocity[1] = rb.velocity.y;
            rbsd.velocity[2] = rb.velocity.z;
        }

        return rbsd;
    }

    public void restoreState(object state)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        RigidbodySaveData rbsd = (RigidbodySaveData)state;

        if (rbsd.isEnabled)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);

        transform.position = new Vector3(rbsd.position[0], rbsd.position[1], rbsd.position[2]);
        transform.rotation = new Quaternion(rbsd.rotation[0], rbsd.rotation[1], rbsd.rotation[2], rbsd.rotation[3]);

        if (rb)
            rb.velocity = new Vector3(rbsd.velocity[0], rbsd.velocity[1], rbsd.velocity[2]);
    }
}

[System.Serializable]
public class RigidbodySaveData
{
    public bool isEnabled;
    public float[] position;
    public float[] rotation;
    public float[] velocity;
}
