using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ePlayerState
{
    Idle,
    Run,
    Die,
    Climb,
    Crouch,
    Shoot,
    ShootCrouch,
    LosingArmour,
}
public enum ePickupType
{
    Armor,
    Dagger,
    Spear,
    Torch,
    Cross,
    Axe,
    Shield,
    None,
}
public enum eWeapon
{
    Dagger,
    Spear,
    Torch,
    Axe,
    Shield,
}


public enum eBossLevel1State
{
    StandStill,
    Walk,
    Jump,
}


public enum eDiabloState
{
    StandStill,
    Flying,
    Walking
}

public enum eZombieState
{
    Appearing, 
    Walking,
    Disappearing,
}

public enum ePlayerArmorState
{
    FullArmor,
    Naked,
    Frog,
}