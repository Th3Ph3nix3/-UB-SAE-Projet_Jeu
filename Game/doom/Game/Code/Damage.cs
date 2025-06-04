using Godot;
using System;
using System.Numerics;
using System.Threading.Tasks;

public partial class Damage : Label
{
    #region methods

    public override void _Ready()
    {
        _ = pop(); // calling the pop method
    }


    public async Task pop()
    {
        var tween = GetTree().CreateTween();
        tween.TweenProperty(this, "scale", new Godot.Vector2(2, 2), 0.1);
        tween.Chain().TweenProperty(this, "scale", new Godot.Vector2(1, 1), 0.1);
        await ToSignal(tween, "finished");
        QueueFree();
    }
    #endregion
}
