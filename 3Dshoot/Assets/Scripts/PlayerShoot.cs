using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] Transform firepoint;
    [SerializeField] GameObject rocketPF;
    [SerializeField] GameObject bulletImpPF;
    [SerializeField] GameObject muzzflashPF;
    [SerializeField] LayerMask layerIgnore;
    float Bcooldown = 0f;
    float Rcooldown = 0f;

    void shootRocket()
    {
        Instantiate(rocketPF, cam.position, cam.rotation);
    }

    void shootBullet()
    {
        Instantiate(muzzflashPF, firepoint.position, cam.rotation);

        float radius = Random.Range(0f, 0.04f);
        float angle = Random.Range(0f, 6.28f);
        Vector3 bSpread = Mathf.Cos(angle) * cam.right * radius + Mathf.Sin(angle) * cam.up * radius;

        RaycastHit impact;

        if (Physics.Raycast(cam.position, cam.forward + bSpread, out impact, 500f, ~layerIgnore))
        {
            Instantiate(bulletImpPF, impact.point, Quaternion.LookRotation(impact.normal));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Rcooldown >= 0.6f)
        {
            Rcooldown = 0f;
            shootRocket();
        }

        if (Input.GetButton("Fire2") && Bcooldown >= 0.1f)
        {
            Bcooldown = 0f;
            shootBullet();
        }

        Rcooldown = Rcooldown < 0.6f ? Rcooldown + Time.deltaTime : Rcooldown;
        Bcooldown = Bcooldown < 0.2f ? Bcooldown + Time.deltaTime : Bcooldown;
    }
}
