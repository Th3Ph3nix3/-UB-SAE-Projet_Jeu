using Godot;
using System;

[GlobalClass] // pour rendre la ressource créable depuis l'éditeur
public partial class EnemyType : Resource
{
	[Export]
	public string title;

	[Export]
	public Texture2D texture;

	[Export]
	public float health;

	[Export]
	public float damage;

	[Export]
	public float speed;

	[Export]
	public int frames = 1;

	[Export]
	public PickupResource[] drops = Array.Empty<PickupResource>();

}
