using Godot;
using System;

public partial class Projectile : Area2D
{

	#region attributes
	public Vector2 direction = Vector2.Right;
	public float speed = 200;
	public float damage = 1;
	public PlayerControl source;
	public Vector2 knockback;
	public AudioStreamPlayer playerShoot;
	#endregion

	#region methods

	public override void _PhysicsProcess(double delta)
	{
		Position += direction * speed * (float)delta; // Move the projectile at a certain speed
	}

	public override void _Ready()
	{
		playerShoot = this.GetNode<AudioStreamPlayer>("PlayerShoot");
		playerShoot.Play(); // play the sound of the projectile when it is created
	}

	public void _on_body_entered(Node2D body)
	{
		if (body is Enemy enemy && body.HasMethod("take_damage"))
		{
			if (source.GetType().GetField("might") != null) // if Source (the player) contains a property might that is not null
			{
				enemy.take_damage(damage * source.might);
				GD.Print("I work in this condition !");
			}
			else
			{
				enemy.take_damage(damage);
				GD.Print("I didn't work in the other condition so i go here");
			}


			enemy.Knockback += direction * 50; // higher value if you want higher knockback. For a gun, 25 is good ig

			QueueFree(); // destroy the projectile if it hits an enemy. Useful if projectile is like a bullet.
		}
	}

	public void _on_screen_exited()
	{
		QueueFree(); // if the projectile is off the screen, destroy it to free memory
	}
	#endregion
}
