using Godot;

/// <summary>
/// Synchronized volume slider - each slider only connects to its specific volume type signal
/// </summary>
public partial class SyncedVolumeSlider : HSlider
{
    [Export] public VolumeType volumeType = VolumeType.Master;
    
    private VolumeManager volumeManager;
    private bool isUpdatingFromManager = false;

    public override void _Ready()
    {
        volumeManager = GetNode<VolumeManager>("/root/VolumeManager");
        ConnectToSpecificSignal();
        
        ValueChanged += OnSliderValueChanged;
        UpdateSliderValue();
    }

    /// <summary>
    /// Each slider connects ONLY to the signal that matches its type
    /// This prevents cross-contamination between different slider types
    /// </summary>
    private void ConnectToSpecificSignal()
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                volumeManager.MasterVolumeChanged += OnVolumeChanged;
                break;
            case VolumeType.Music:
                volumeManager.MusicVolumeChanged += OnVolumeChanged;
                break;
            case VolumeType.Click:
                volumeManager.ClickVolumeChanged += OnVolumeChanged;
                break;
            case VolumeType.Monster:
                volumeManager.MonsterVolumeChanged += OnVolumeChanged;
                break;
            case VolumeType.Player:
                volumeManager.PlayerVolumeChanged += OnVolumeChanged;
                break;
        }
    }

    private void OnSliderValueChanged(double value)
    {
        if (isUpdatingFromManager) return;
        
        // Update only the specific volume type
        switch (volumeType)
        {
            case VolumeType.Master:
                volumeManager.SetMasterVolume((float)value);
                break;
            case VolumeType.Music:
                volumeManager.SetMusicVolume((float)value);
                break;
            case VolumeType.Click:
                volumeManager.SetClickVolume((float)value);
                break;
            case VolumeType.Monster:
                volumeManager.SetMonsterVolume((float)value);
                break;
            case VolumeType.Player:
                volumeManager.SetPlayerVolume((float)value);
                break;
        }
    }

    /// <summary>
    /// Single handler for volume changes - only called for the correct volume type
    /// </summary>
    private void OnVolumeChanged(float value)
    {
        isUpdatingFromManager = true;
        Value = value;
        isUpdatingFromManager = false;
    }

    private void UpdateSliderValue()
    {
        isUpdatingFromManager = true;
        
        switch (volumeType)
        {
            case VolumeType.Master:
                Value = volumeManager.MasterVolume;
                break;
            case VolumeType.Music:
                Value = volumeManager.MusicVolume;
                break;
            case VolumeType.Click:
                Value = volumeManager.ClickVolume;
                break;
            case VolumeType.Monster:
                Value = volumeManager.MonsterVolume;
                break;
            case VolumeType.Player:
                Value = volumeManager.PlayerVolume;
                break;
        }
        
        isUpdatingFromManager = false;
    }

    /// <summary>
    /// Disconnect only the signal this slider was connected to
    /// </summary>
    public override void _ExitTree()
    {
        if (volumeManager != null)
        {
            switch (volumeType)
            {
                case VolumeType.Master:
                    volumeManager.MasterVolumeChanged -= OnVolumeChanged;
                    break;
                case VolumeType.Music:
                    volumeManager.MusicVolumeChanged -= OnVolumeChanged;
                    break;
                case VolumeType.Click:
                    volumeManager.ClickVolumeChanged -= OnVolumeChanged;
                    break;
                case VolumeType.Monster:
                    volumeManager.MonsterVolumeChanged -= OnVolumeChanged;
                    break;
                case VolumeType.Player:
                    volumeManager.PlayerVolumeChanged -= OnVolumeChanged;
                    break;
            }
        }
    }
}
