using Godot;
using System;

public partial class Projectile : Area2D
{

	#region attributes
	public Vector2 direction = Vector2.Right;
	public float speed = 200;
	public float damage = 1;

	public Vector2 knockback;
	#endregion
	#region methods

	public override void _PhysicsProcess(double delta)
	{
		Position += direction * speed * (float)delta; // Move the projectile at a certain speed
	}

	public void _on_body_entered(Node2D body)
	{
		if (body is Enemy enemy)
		{
			enemy.take_damage(damage);
			enemy.knockback += direction * 50; // higher value if you want higher knockback. For a gun, 25 is good ig

			QueueFree(); // destroy the projectile if it hits an enemy. Useful if projectile is like a bullet.
		}
	}

	public void _on_screen_exited()
	{
		QueueFree(); // if the projectile is off the screen, destroy it to free memory
	}
	#endregion
}
