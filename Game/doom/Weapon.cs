using Godot;
using System;
using System.Xml.Resolvers;

[GlobalClass]
public partial class Weapon : Resource
{
    [Export]
    public string title; // name of the weapon

    [Export]
    public Texture2D texture; // icon of the weapon


    // projectile
    [Export]
    public float damage; // damage of the projectile
    [Export]
    public float cooldown; // cooldown until next projectile

    [Export]
    public float speed; // speed of the projectile

    [Export]
    public PackedScene projectile_node = GD.Load<PackedScene>("res://projectile.tscn");

    public void activate(PlayerControl _source, Enemy _target, SceneTree _scene_tree) // quels sont les types ? Je sais pas, j'ai mis au hasard un peu
    {
        //
    }
}
