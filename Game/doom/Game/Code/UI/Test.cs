using Godot;
using System;

public partial class Test : TextureButton
{
    public override void _Ready()
    {
        // Connect the pressed signal
        Pressed += () => GD.PrintErr("Pressed signal received on: " + Name);

        // Connect the gui_input signal
        GuiInput += _on_gui_input;
    }

    private void _on_gui_input(InputEvent @event)
    {
        GD.PrintErr($"{Name} received gui_input: {@event}");
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            GD.PrintErr($"{Name} Mouse button pressed: {mouseEvent.ButtonIndex}");
        }
    }

    public override void _Draw()
    {
        // Draw a visible red border for debugging
        DrawRect(new Rect2(Vector2.Zero, Size), new Color(1, 0, 0), false, 3);
    }

    public override void _Process(double delta)
    {
        // Continuously redraw to keep the border visible if resized
        QueueRedraw();
    }
}