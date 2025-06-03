using Godot;
using System;

public partial class MainMenu : Node2D
{
	private AudioStreamPlayer clickcliksound;
	#region methods
	public void _on_start_pressed()
	{
		clickcliksound = GetNode<AudioStreamPlayer>("ClickClickSound");
		clickcliksound.Play();
		GetTree().ChangeSceneToFile("res://Game/Scenes/Game_Test.tscn");
	}

	public void _on_quit_pressed()
	{
		clickcliksound = GetNode<AudioStreamPlayer>("ClickClickSound");
		clickcliksound.Play();
		GetTree().Quit();
	}
	#endregion
}
