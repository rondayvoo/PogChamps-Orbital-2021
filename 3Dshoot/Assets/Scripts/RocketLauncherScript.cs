using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherScript : BaseWeaponScript
{
    public GameObject rocketPF;
    [SerializeField]
    private AudioSource shootSound;


    public override void primaryFire()
    {
        Transform cam = Camera.main.transform;
        Instantiate(rocketPF, cam.position, cam.rotation);
        muzzleFlash.Play();
        shootSound.Play();
    }

    public override void secondaryFire()
    {

    }
}
