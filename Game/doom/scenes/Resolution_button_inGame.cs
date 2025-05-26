using Godot;
using System;

public partial class Resolution_button_inGame : VBoxContainer
{

	private bool _isFullscreen = true;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void _on_1024x546_pressed()
	{
		var window = GetWindow();
		window.Size = new Vector2I(1024, 546);
	}
	public void _on_1280x720_pressed()
	{
		var window = GetWindow();
		window.Size = new Vector2I(1280, 720);
	}
	public void _on_1600x900_pressed()
	{
		var window = GetWindow();
		window.Size = new Vector2I(1600, 900);
	}
	public void _on_1920x1080_pressed()
	{
		var window = GetWindow();
		window.Size = new Vector2I(1920, 1080);
	}
	public void _on_1920x1200_pressed()
	{
		var window = GetWindow();
		window.Size = new Vector2I(1920, 1200);
	}
}
