using Godot;
using System;

public partial class Pickups : Area2D
{
	#region Attributes
	public bool can_follow = false;

	public Vector2 direction;
	public float speed = 450;

	[Export]
	public PickupResource type;

	#endregion
	#region Setters / Getters

	[Export]
	public CharacterBody2D _player_reference;

	public CharacterBody2D player_reference
	{
		get => _player_reference;
		set
		{
			_player_reference = value;

			if (type != null)
			{
				if (value is Player playerControl)
				{
					type.PlayerReference = playerControl;
				}
				else
				{
					GD.PrintErr("Le joueur assigné n'est pas un PlayerControl !");
				}
			}
		}
	}

	#endregion
	#region Ready and Physics Process

	public override void _Ready()
	{
		var sprite2D = GetNode<Sprite2D>("Sprite2D");
		var collisionShape2D = GetNode <CollisionShape2D>("CollisionShape2D");

		if (type != null)
		{
			sprite2D.Texture = type.Icon;
			sprite2D.Scale = new Godot.Vector2(3f, 3f); // upscale the sprite of the resource
			collisionShape2D.Scale = new Godot.Vector2(5f, 5f); // upscale the collision shape of the resource so the player needs to be less closer to pick it up
		}
		else
		{
			GD.PrintErr("Le type du pickup n'est pas assigné dans _Ready !");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (player_reference != null && can_follow)
		{
			direction = (player_reference.Position - Position).Normalized();
			Position += direction * speed * (float)delta;
		}
	}
	#endregion

	#region Methods
	public void follow(CharacterBody2D _target)
	{
		player_reference = _target;
		can_follow = true;
	}

	public void _on_body_entered(Node2D body)
	{
		if (body == player_reference)
		{
			type?.Activate();
			QueueFree();
		}
	}

	#endregion
}
