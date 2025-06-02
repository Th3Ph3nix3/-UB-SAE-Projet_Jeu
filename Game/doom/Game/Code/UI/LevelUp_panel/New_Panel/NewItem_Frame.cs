using System;
using Godot;

public partial class NewItem_Frame : TextureButton
{
    #region Attributes

    /// <summary>
    /// Item data to give to the new item frame.
    /// </summary>
    private Items_Data _itemData;

    #endregion

    #region Methods

    /// <summary>
    /// Custom constructor to instantiate a new item frame from its scene.
    /// </summary>
    /// <param name="itemData">Item data to give to the new item frame.</param>
    /// <param name="options">Reference of the Options class.</param>
    /// <returns>Returns a reference the the newly instantiated item frame.</returns>
    static public NewItem_Frame new_NewItem_Frame(Items_Data itemData)
    {
        NewItem_Frame newItem_frame = GD.Load<PackedScene>("res://Game/Scenes/UI/Upgrade_UI/NewItem_Frame.tscn").Instantiate<NewItem_Frame>();
        newItem_frame._itemData = itemData;
        return newItem_frame;
    }

    /// <summary>
    /// Called when the node is instantiated. Set the label and description of the passive.
    /// </summary>
    public override void _Ready()
    {
        Label label = GetNode<Label>("Label");
        Label description = GetNode<Label>("Description");

        TextureNormal = _itemData.texture;
        label.Text = "New !";
        description.Text = _itemData.Upgrades[_itemData.level].description;
    }

    /// <summary>
    /// Called when the player clicks on the slot to add the new passive item to the player.
    /// </summary>
    /// <param name="inputEvent">Event inputed.</param>
    public void _on_gui_input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("click"))
        {
            GD.PrintErr("NewItem_Frame : Player clicked on the new item frame to add the passive item.");

            if (_itemData != null)
            {
                Global.PlayerManager.Player.AddPassive(_itemData); // Add the new passive item to the player
                UI.LevelUp_Panel.Close(); // Close the options panel
            }
            else
            {
                GD.PrintErr("NewItem_Frame : Item data cannot be null.");
            }
        }
    }

    #endregion
}