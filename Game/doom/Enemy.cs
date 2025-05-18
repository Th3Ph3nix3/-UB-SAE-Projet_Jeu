using Godot;
using System;
using System.Drawing;
using System.Numerics;

public partial class Enemy : CharacterBody2D
{
    Godot.Vector2 direction;
    public float speed = 75;
    public float damage;
    public Godot.Vector2 knockback;
    public float separation;

    public float _health;

    public float health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health <= 0)
            {
                QueueFree();
            }
        }
    }

    public PackedScene damage_popup_node = GD.Load<PackedScene>("res://damage.tscn");

    public bool _elite = false;

    public bool elite
    {
        get => _elite;
        set
        {
            _elite = value;
            if (Sprite2D != null && value)  // cf ligne 57
            {
                var mat = GD.Load<ShaderMaterial>("res://Shaders/Rainbow.tres");
                Sprite2D.Material = mat;
                Scale = new Godot.Vector2(5f, 5f);
            }
        }
    }

    [Export]
    public CharacterBody2D player_reference;

    [Export]
    public Sprite2D Sprite2D;

    private EnemyType type;

    public EnemyType Type
    {
        get => type;
        set
        {
            type = value;
            UpdateSpriteTexture();
            damage = value.damage;
            health = value.health;
        }
    }

    private void UpdateSpriteTexture()
    {
        if (Sprite2D != null && type != null)
            Sprite2D.Texture = type.texture;
    }

    public override void _Ready()
    {
        Sprite2D = GetNode<Sprite2D>("Sprite2D");
        UpdateSpriteTexture();

        if (_elite && Sprite2D.Material == null) // cf ligne 18
        {
            var mat = GD.Load<ShaderMaterial>("res://Shaders/Rainbow.tres");
            Sprite2D.Material = mat;
            Sprite2D.Scale = new Godot.Vector2(5f, 5f);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        check_separation(delta);
        knockback_update(delta);
    }

    public void check_separation(double _delta)
    {
        separation = (player_reference.Position - Position).Length();
        if (separation >= 500 && !elite)
        {
            QueueFree();
        }
        var player = player_reference as PlayerControl;
        if (separation < player.nearest_enemy_distance)
        {
            player.nearest_enemy_distance = separation;
            player.nearest_enemy = this;
        }
    }

    public void knockback_update(double delta)
    {
        Godot.Vector2 targetPosition = player_reference.Position;
        Godot.Vector2 moveDirection = targetPosition - Position;
        moveDirection = moveDirection.Normalized();

        Velocity = moveDirection * speed;

        knockback = MoveToward(knockback, Godot.Vector2.Zero, 1);
        Velocity += knockback;

        var collider = MoveAndCollide(Velocity * (float)delta);
        if (collider != null)
        {
            var hitNode = collider.GetCollider();
            var hitEnemy = hitNode as Enemy;
            if (hitEnemy != null)
            {
                var hitEnemyPosition = hitEnemy.GlobalPosition;
                hitEnemy.knockback = (hitEnemyPosition - GlobalPosition).Normalized() * 50;
            }
        }
    }

    // reproduction du fonctionnement de MoveToward
    public Godot.Vector2 MoveToward(Godot.Vector2 current, Godot.Vector2 target, float maxDistanceDelta)
    {
        Godot.Vector2 direction = target - current;
        float distance = direction.Length();

        if (distance <= maxDistanceDelta || distance == 0)
        {
            return target;
        }

        direction = direction.Normalized();

        return current + direction * maxDistanceDelta;
    }

    // function to isntantiate damage popup & add it to the scene
    public void damage_popup(float amount)
    {
        var popup = damage_popup_node.Instantiate<Damage>();
        popup.Text = amount.ToString();
        popup.Position = Position + new Godot.Vector2(-50, -50);
        GetTree().CurrentScene.AddChild(popup);
    }

    public void take_damage(float amount)
    {
        var tween = GetTree().CreateTween();
        tween.TweenProperty(Sprite2D, "modulate", new Godot.Color(3, (float)0.25, (float)0.25), 0.1);
        tween.Chain().TweenProperty(Sprite2D, "modulate", new Godot.Color(1, 1, 1), 0.1);
        tween.BindNode(this);

        damage_popup(amount);
        health -= amount;
    }
}
