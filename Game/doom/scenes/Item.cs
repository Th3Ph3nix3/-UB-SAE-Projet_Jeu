using Godot;
using System;

[GlobalClass]
public partial class Item : Resource
{
    [Export]
    public string title;

    [Export]
    public Texture2D texture;
    public int level = 1;
}
