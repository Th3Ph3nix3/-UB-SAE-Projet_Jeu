using Godot;

public partial class MonsterVolumeSlider : HSlider
{
    [Export]
    public string bus_name = "Monster";
    
    private int bus_index;

    public override void _Ready()
    {
        bus_index = AudioServer.GetBusIndex(bus_name);
        this.ValueChanged += OnValueChanged;
    }

    private void OnValueChanged(double value)
    {
        AudioServer.SetBusVolumeDb(
            bus_index,
            Mathf.LinearToDb((float)value) // Utilise directement Mathf.LinearToDb
        );
    }
}
