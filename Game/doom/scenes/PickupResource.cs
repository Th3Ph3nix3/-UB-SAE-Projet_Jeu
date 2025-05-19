using Godot;

[GlobalClass]
public partial class PickupResource : Resource
{
	#region attributes
	[Export]
	public string Title = "";

	[Export]
	public Texture2D Icon;

	[Export(PropertyHint.MultilineText)]
	public string Description = "";

	public PlayerControl PlayerReference;

	#endregion
	#region methods

	public virtual void Activate()
	{
		GD.Print(Title + " picked up.");
	}
	#endregion
}
