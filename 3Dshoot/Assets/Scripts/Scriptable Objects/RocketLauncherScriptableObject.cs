using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/Rocket Launcher")]
public class RocketLauncherScriptableObject : WeaponScriptableObject
{
    public GameObject rocketPF;

    public override void primaryFire(Transform cam)
    {
        Instantiate(rocketPF, cam.position, cam.rotation);
    }
}

//[System.Serializable]
//public class RocketLauncher : Weapon
//{
//    public WeaponScriptableObject<RocketLauncher> AR;
//
//    public Weapon(WeaponScriptableObject<RocketLauncher> blueprint)
//    {
//        AR = blueprint;
//    }
//}
