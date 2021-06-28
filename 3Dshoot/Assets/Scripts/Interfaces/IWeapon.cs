using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void primaryFire();
    void secondaryFire();
    float fireDelay { get; }
    int currAmmo { get; set; }
}
