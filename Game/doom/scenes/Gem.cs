using Godot;

[GlobalClass]
public partial class Gem : PickupResource
{
	#region attributes
	[Export]
	public int XP { get; set; }

	#endregion
	#region methods

	public override void Activate()
	{
		base.Activate();
		GD.Print("+" + XP + " XP");

		if (PlayerReference is PlayerControl player)
		{
			player.Gain_XP(XP);
		}
		else
		{
			GD.PrintErr("PlayerReference n'est pas un PlayerControl !");
		}
	}
	#endregion
}
