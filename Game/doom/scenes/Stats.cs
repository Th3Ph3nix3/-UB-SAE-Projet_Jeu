using Godot;
using System;

[GlobalClass]
public partial class Stats : Resource
{
    [Export(PropertyHint.MultilineText)]
    public string description;
    [Export]
    public float max_health;
    [Export]
    public float recovery;
    [Export]
    public float armor;
    [Export]
    public float movement_speed;
    [Export]
    public float might;
    [Export]
    public float area;
    [Export]
    public float magnet;
    [Export]
    public int revival;
    [Export]
    public float growth;
}
