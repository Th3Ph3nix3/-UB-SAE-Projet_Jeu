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
}
