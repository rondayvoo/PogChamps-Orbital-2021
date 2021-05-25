using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item/Weapon/Rocket Launcher")]
public class RocketLauncherScriptableObject : WeaponScriptableObject
{
    public GameObject rocketPF;
    public ProjectileScriptableObject baseProj;
    public ExplosionScriptableObject baseExp;

    public override void primaryFire(Transform cam)
    {
        GameObject proj = Instantiate(rocketPF, cam.position, cam.rotation);
        proj.GetComponent<RocketScience>().baseProj = baseProj;
        proj.GetComponent<RocketScience>().baseExp = baseExp;
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
