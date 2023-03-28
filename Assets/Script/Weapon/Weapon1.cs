using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon1 : Gun
{
    protected override void GunAttack()
    {
        base.GunAttack();
        AudioController.instance.PistolAudio();
    }
}
