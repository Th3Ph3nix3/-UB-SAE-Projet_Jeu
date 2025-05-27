using Godot;
using System;

public partial class MainMenu : Node2D
{
	#region methods
	public void _on_start_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/Game_Test.tscn");
	}

	public void _on_quit_pressed()
	{
		GetTree().Quit();
	}
	#endregion
}
