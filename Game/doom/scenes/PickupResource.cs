using Godot;

[GlobalClass]
public partial class PickupResource : Resource
{
	#region attributes
	[Export]
	public string Title { get; set; } = "";

	[Export]
	public Texture2D Icon { get; set; }

	[Export(PropertyHint.MultilineText)]
	public string Description { get; set; } = "";

	public CharacterBody2D PlayerReference { get; set; }

	#endregion
	#region methods

	public virtual void Activate()
	{
		GD.Print(Title + " picked up.");
	}
	#endregion
}
