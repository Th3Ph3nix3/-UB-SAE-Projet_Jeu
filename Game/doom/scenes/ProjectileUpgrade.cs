using Godot;
using System;

[GlobalClass]
public partial class ProjectileUpgrade : Upgrade
{
    #region attributes

    [Export]
    public float speed; //Exclusive to projectile upgrades

    #endregion
}
