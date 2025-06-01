// using Godot;
// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Linq;
// using System.Security.AccessControl;

// public partial class Circular : Weapon
// {
// 	[Export]
// 	private float anglular_speed = 20;

// 	[Export]
// 	private int radius = 20;

// 	[Export]
// 	private int amount = 1;

// 	private Area2D[] projectile_reference = Array.Empty<Area2D>();
// 	private float angle;

// 	public void add_to_player()
// 	{
// 		Area2D projectile = Projectile_node.Instantiate<Area2D>();

// 		projectile.Speed = 0;
// 		projectile.Damage = Damage;
// 		projectile.Source = Source;
// 		projectile.Hide();

// 		projectile_reference.Append(projectile);
// 		Source.CallDeferred("add_child", projectile);
// 	}

// 	public void update(float delta)
// 	{
// 		angle += anglular_speed * delta;

// 		for (int i = 0; i < projectile_reference.Length; i++)
// 		{
// 			var offset = i * (360 / amount);

// 			projectile_reference[i].Position = radius * new Vector2(Mathf.Cos(Mathf.DegToRad(angle + offset)), Mathf.Sin(Mathf.DegToRad(angle + offset))); // Set the position of the projectile

// 			projectile_reference[i].Show();
// 		}
// 	}

// }
