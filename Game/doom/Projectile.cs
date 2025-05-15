using Godot;
using System;

public partial class Projectile : Area2D
{
	public Vector2 direction = Vector2.Right;
	public float speed = 200;
	public float damage = 1;

	public void _PhysicsProcess(float delta)
	{
		Position += direction * speed * delta;
	}

	public void _on_body_entered(PlayerControl body)
	{
		if (body.HasMethod("take_damage"))
		{
			body.take_damage(damage);
		}
	}

	public void _on_screen_exited()
	{
		QueueFree();
	}
}
