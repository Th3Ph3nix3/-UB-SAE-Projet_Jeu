using Godot;

public partial class SyncedVolumeSlider : HSlider
{
    #region Exported Properties
    /// <summary>
    /// The type of volume this slider controls (Master, Music, Click, Monster, or Player).
    /// 
    /// This property determines:
    /// - Which audio bus this slider affects
    /// - Which VolumeManager signal this slider listens to
    /// - Which other sliders this slider synchronizes with
    /// 
    /// Set this in the Godot inspector to match your desired audio bus.
    /// Example: Set to VolumeType.Music for a music volume slider.
    /// </summary>
    [Export] public VolumeType volumeType = VolumeType.Master;
    #endregion

    #region Private Fields
    /// <summary>
    /// Reference to the global VolumeManager autoload singleton.
    /// Used to set volume values and listen for volume change notifications.
    /// </summary>
    private VolumeManager volumeManager;
    
    /// <summary>
    /// Flag to prevent infinite loops when updating slider values programmatically.
    /// 
    /// When true, the OnSliderValueChanged method ignores user input to prevent:
    /// 1. User moves slider A
    /// 2. VolumeManager updates volume
    /// 3. VolumeManager signals all sliders of that type
    /// 4. Slider A receives its own change back and tries to update again
    /// 
    /// This flag breaks that cycle by ignoring changes when they come from the manager.
    /// </summary>
    private bool isUpdatingFromManager = false;
    #endregion

    #region Godot Lifecycle
    /// <summary>
    /// Initializes the slider by connecting to the VolumeManager and setting up event handlers.
    /// 
    /// This method:
    /// 1. Gets a reference to the global VolumeManager autoload
    /// 2. Connects to the appropriate volume change signal based on volumeType
    /// 3. Sets up the slider's ValueChanged event handler
    /// 4. Initializes the slider's value to match the current volume in VolumeManager
    /// 
    /// Called automatically by Godot when the node enters the scene tree.
    /// </summary>
    public override void _Ready()
    {
        // Get reference to the global VolumeManager autoload
        volumeManager = GetNode<VolumeManager>("/root/VolumeManager");
        
        // Connect to the specific signal that matches this slider's type
        // This ensures only relevant volume changes affect this slider
        ConnectToSpecificSignal();
        
        // Listen for user interactions with this slider
        ValueChanged += OnSliderValueChanged;
        
        // Set the initial slider value to match the current volume
        UpdateSliderValue();
    }

    /// <summary>
    /// Cleanup method called when the node is about to be removed from the scene tree.
    /// 
    /// Disconnects from VolumeManager signals to prevent:
    /// - Memory leaks from dangling signal connections
    /// - Errors when VolumeManager tries to call methods on a freed object
    /// - Potential crashes or undefined behavior
    /// 
    /// Called automatically by Godot when the node exits the scene tree.
    /// </summary>
    public override void _ExitTree()
    {
        // Only disconnect if we have a valid VolumeManager reference
        if (volumeManager != null)
        {
            // Disconnect from the specific signal this slider was connected to
            // This prevents memory leaks and potential errors
            DisconnectFromSpecificSignal();
        }
    }
    #endregion

    #region Signal Connection Management
    /// <summary>
    /// Connects this slider to the specific VolumeManager signal that matches its type.
    /// 
    /// This method ensures that each slider only responds to changes in its own volume type:
    /// - Master sliders only respond to MasterVolumeChanged
    /// - Music sliders only respond to MusicVolumeChanged
    /// - And so on...
    /// 
    /// This selective connection prevents cross-contamination between different slider types.
    /// </summary>
    private void ConnectToSpecificSignal()
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                // Connect to Master volume changes only
                volumeManager.MasterVolumeChanged += OnVolumeChanged;
                break;
            case VolumeType.Music:
                // Connect to Music volume changes only
                volumeManager.MusicVolumeChanged += OnVolumeChanged;
                break;
            case VolumeType.Click:
                // Connect to Click volume changes only
                volumeManager.ClickVolumeChanged += OnVolumeChanged;
                break;
            case VolumeType.Monster:
                // Connect to Monster volume changes only
                volumeManager.MonsterVolumeChanged += OnVolumeChanged;
                break;
            case VolumeType.Player:
                // Connect to Player volume changes only
                volumeManager.PlayerVolumeChanged += OnVolumeChanged;
                break;
        }
    }

    /// <summary>
    /// Disconnects this slider from its specific VolumeManager signal.
    /// 
    /// This is the cleanup counterpart to ConnectToSpecificSignal().
    /// Called during _ExitTree() to prevent memory leaks and errors.
    /// </summary>
    private void DisconnectFromSpecificSignal()
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
    #endregion

    #region Event Handlers
    /// <summary>
    /// Called when the user moves this slider (drag, click, keyboard input).
    /// 
    /// This method:
    /// 1. Checks if the change is coming from the VolumeManager (ignore if so)
    /// 2. Updates the appropriate volume in VolumeManager based on slider type
    /// 3. VolumeManager then notifies all other sliders of the same type
    /// 
    /// The loop prevention is crucial:
    /// User Input → This Slider → VolumeManager → Other Sliders (not this one again)
    /// </summary>
    /// <param name="value">New slider value (0.0 to 1.0)</param>
    private void OnSliderValueChanged(double value)
    {
        // Prevent infinite loops: ignore changes when they come from VolumeManager
        if (isUpdatingFromManager) return;
        
        // Update the appropriate volume type based on this slider's configuration
        // The VolumeManager will then notify all other sliders of the same type
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
    /// Called when the VolumeManager notifies this slider of a volume change.
    /// 
    /// This happens when:
    /// - Another slider of the same type was moved by the user
    /// - Volume was changed programmatically
    /// - Settings were loaded from file
    /// 
    /// The method updates this slider's visual position to match the new volume
    /// while preventing it from triggering its own OnSliderValueChanged event.
    /// </summary>
    /// <param name="value">New volume value to display (0.0 to 1.0)</param>
    private void OnVolumeChanged(float value)
    {
        // Set flag to prevent infinite loops
        isUpdatingFromManager = true;
        
        // Update the slider's visual position to match the new volume
        Value = value;
        
        // Clear the flag to allow normal user interaction again
        isUpdatingFromManager = false;
    }
    #endregion

    #region Private Helper Methods
    /// <summary>
    /// Initializes the slider's value to match the current volume in VolumeManager.
    /// 
    /// This is called during _Ready() to ensure the slider shows the correct initial value.
    /// Without this, sliders might show default values (like 0.5) even if the actual
    /// volume is different (like 0.8 from loaded settings).
    /// 
    /// Uses the same loop prevention mechanism as OnVolumeChanged.
    /// </summary>
    private void UpdateSliderValue()
    {
        // Prevent the initial value update from triggering OnSliderValueChanged
        isUpdatingFromManager = true;
        
        // Set the slider value based on the current volume for this type
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
        
        // Clear the flag to allow normal operation
        isUpdatingFromManager = false;
    }
    #endregion
}
