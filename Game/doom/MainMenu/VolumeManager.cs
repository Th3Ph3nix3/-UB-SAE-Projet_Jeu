using System;
using Godot;

public partial class VolumeManager : Node
{
    #region Signals - One signal per audio bus type
    /// <summary>
    /// Emitted when the Master volume changes. All Master volume sliders listen to this signal.
    /// </summary>
    /// <param name="value">New volume value (0.0 to 1.0, linear scale)</param>
    [Signal] public delegate void MasterVolumeChangedEventHandler(float value);
    
    /// <summary>
    /// Emitted when the Music volume changes. All Music volume sliders listen to this signal.
    /// </summary>
    /// <param name="value">New volume value (0.0 to 1.0, linear scale)</param>
    [Signal] public delegate void MusicVolumeChangedEventHandler(float value);
    
    /// <summary>
    /// Emitted when the Click volume changes. All Click volume sliders listen to this signal.
    /// </summary>
    /// <param name="value">New volume value (0.0 to 1.0, linear scale)</param>
    [Signal] public delegate void ClickVolumeChangedEventHandler(float value);
    
    /// <summary>
    /// Emitted when the Monster volume changes. All Monster volume sliders listen to this signal.
    /// </summary>
    /// <param name="value">New volume value (0.0 to 1.0, linear scale)</param>
    [Signal] public delegate void MonsterVolumeChangedEventHandler(float value);
    
    /// <summary>
    /// Emitted when the Player volume changes. All Player volume sliders listen to this signal.
    /// </summary>
    /// <param name="value">New volume value (0.0 to 1.0, linear scale)</param>
    [Signal] public delegate void PlayerVolumeChangedEventHandler(float value);
    #endregion

    #region Volume Properties - Current volume states (linear scale 0.0-1.0)
    /// <summary>
    /// Current Master volume level (0.0 = silent, 1.0 = full volume).
    /// This affects all audio in the game as Master is the root bus.
    /// </summary>
    public float MasterVolume { get; private set; } = 1.0f;
    
    /// <summary>
    /// Current Music volume level (0.0 = silent, 1.0 = full volume).
    /// Controls background music and ambient audio.
    /// </summary>
    public float MusicVolume { get; private set; } = 1.0f;
    
    /// <summary>
    /// Current Click volume level (0.0 = silent, 1.0 = full volume).
    /// Controls UI click sounds and interface audio feedback.
    /// </summary>
    public float ClickVolume { get; private set; } = 1.0f;
    
    /// <summary>
    /// Current Monster volume level (0.0 = silent, 1.0 = full volume).
    /// Controls enemy sounds, monster roars, and creature audio effects.
    /// </summary>
    public float MonsterVolume { get; private set; } = 1.0f;
    
    /// <summary>
    /// Current Player volume level (0.0 = silent, 1.0 = full volume).
    /// Controls player character sounds, footsteps, and action audio.
    /// </summary>
    public float PlayerVolume { get; private set; } = 1.0f;
    #endregion

    #region Private Fields - Internal state management
    /// <summary>Audio bus index for Master bus (obtained from AudioServer)</summary>
    private int masterBusIndex;
    
    /// <summary>Audio bus index for Music bus (obtained from AudioServer)</summary>
    private int musicBusIndex;
    
    /// <summary>Audio bus index for Click bus (obtained from AudioServer)</summary>
    private int clickBusIndex;
    
    /// <summary>Audio bus index for Monster bus (obtained from AudioServer)</summary>
    private int monsterBusIndex;
    
    /// <summary>Audio bus index for Player bus (obtained from AudioServer)</summary>
    private int playerBusIndex;
    
    /// <summary>File path where volume settings are saved in the user directory</summary>
    private const string SETTINGS_FILE_PATH = "user://volume_settings.cfg";
    #endregion

    #region Godot Lifecycle
    /// <summary>
    /// Initializes the VolumeManager by:
    /// 1. Retrieving audio bus indices from the AudioServer
    /// 2. Loading previously saved volume settings
    /// 3. Applying those settings to both the manager and AudioServer
    /// 
    /// This method is called automatically when the autoload is initialized.
    /// </summary>
    public override void _Ready()
    {
        // Retrieve audio bus indices for performance optimization
        // These indices are used instead of string lookups during runtime
        masterBusIndex = AudioServer.GetBusIndex("Master");
        musicBusIndex = AudioServer.GetBusIndex("Music");
        clickBusIndex = AudioServer.GetBusIndex("Click");
        monsterBusIndex = AudioServer.GetBusIndex("Monster");
        playerBusIndex = AudioServer.GetBusIndex("Player");
        
        // Load and apply previously saved volume settings
        // This ensures volume settings persist between game sessions
        LoadVolumeSettings();
    }
    #endregion

    #region Public Volume Setters - Main API for volume control
    /// <summary>
    /// Sets the Master volume and immediately updates the audio bus.
    /// This affects all audio in the game as Master is the root bus.
    /// Automatically saves settings and notifies all listening sliders.
    /// </summary>
    /// <param name="value">Volume level (0.0 = silent, 1.0 = full volume)</param>
    public void SetMasterVolume(float value)
    {
        // Clamp to valid range to prevent audio distortion
        MasterVolume = Mathf.Clamp(value, 0.0f, 1.0f);
        
        // Apply to AudioServer (convert linear to decibels for proper audio scaling)
        AudioServer.SetBusVolumeDb(masterBusIndex, Mathf.LinearToDb(MasterVolume));
        
        // Notify all Master volume sliders about the change
        EmitSignal(SignalName.MasterVolumeChanged, MasterVolume);
        
        // Persist the new setting to disk
        SaveVolumeSettings();
    }

    /// <summary>
    /// Sets the Music volume and immediately updates the audio bus.
    /// Controls background music and ambient audio levels.
    /// Automatically saves settings and notifies all listening sliders.
    /// </summary>
    /// <param name="value">Volume level (0.0 = silent, 1.0 = full volume)</param>
    public void SetMusicVolume(float value)
    {
        MusicVolume = Mathf.Clamp(value, 0.0f, 1.0f);
        AudioServer.SetBusVolumeDb(musicBusIndex, Mathf.LinearToDb(MusicVolume));
        EmitSignal(SignalName.MusicVolumeChanged, MusicVolume);
        SaveVolumeSettings();
    }

    /// <summary>
    /// Sets the Click volume and immediately updates the audio bus.
    /// Controls UI interaction sounds and interface audio feedback.
    /// Automatically saves settings and notifies all listening sliders.
    /// </summary>
    /// <param name="value">Volume level (0.0 = silent, 1.0 = full volume)</param>
    public void SetClickVolume(float value)
    {
        ClickVolume = Mathf.Clamp(value, 0.0f, 1.0f);
        AudioServer.SetBusVolumeDb(clickBusIndex, Mathf.LinearToDb(ClickVolume));
        EmitSignal(SignalName.ClickVolumeChanged, ClickVolume);
        SaveVolumeSettings();
    }

    /// <summary>
    /// Sets the Monster volume and immediately updates the audio bus.
    /// Controls enemy sounds, creature roars, and monster audio effects.
    /// Automatically saves settings and notifies all listening sliders.
    /// </summary>
    /// <param name="value">Volume level (0.0 = silent, 1.0 = full volume)</param>
    public void SetMonsterVolume(float value)
    {
        MonsterVolume = Mathf.Clamp(value, 0.0f, 1.0f);
        AudioServer.SetBusVolumeDb(monsterBusIndex, Mathf.LinearToDb(MonsterVolume));
        EmitSignal(SignalName.MonsterVolumeChanged, MonsterVolume);
        SaveVolumeSettings();
    }

    /// <summary>
    /// Sets the Player volume and immediately updates the audio bus.
    /// Controls player character sounds, footsteps, and action audio.
    /// Automatically saves settings and notifies all listening sliders.
    /// </summary>
    /// <param name="value">Volume level (0.0 = silent, 1.0 = full volume)</param>
    public void SetPlayerVolume(float value)
    {
        PlayerVolume = Mathf.Clamp(value, 0.0f, 1.0f);
        AudioServer.SetBusVolumeDb(playerBusIndex, Mathf.LinearToDb(PlayerVolume));
        EmitSignal(SignalName.PlayerVolumeChanged, PlayerVolume);
        SaveVolumeSettings();
    }
    #endregion

    #region Settings Management - Persistence layer
    /// <summary>
    /// Saves all current volume settings to a configuration file in the user directory.
    /// The file is automatically created if it doesn't exist.
    /// This is called automatically whenever a volume setting changes.
    /// </summary>
    private void SaveVolumeSettings()
    {
        var config = new ConfigFile();
        
        // Store all volume values in the "audio" section
        config.SetValue("audio", "master_volume", MasterVolume);
        config.SetValue("audio", "music_volume", MusicVolume);
        config.SetValue("audio", "click_volume", ClickVolume);
        config.SetValue("audio", "monster_volume", MonsterVolume);
        config.SetValue("audio", "player_volume", PlayerVolume);
        
        // Save to user directory (persists between game sessions)
        config.Save(SETTINGS_FILE_PATH);
    }

    /// <summary>
    /// Loads volume settings from the configuration file.
    /// If no file exists, default values (1.0 for all volumes) are used.
    /// This is called automatically during initialization.
    /// </summary>
    private void LoadVolumeSettings()
    {
        var config = new ConfigFile();
        
        // Attempt to load the settings file
        if (config.Load(SETTINGS_FILE_PATH) == Error.Ok)
        {
            // Load each volume setting with fallback defaults
            // ApplyVolumeDirectly is used to avoid triggering signals during initialization
            ApplyVolumeDirectly(VolumeType.Master, (float)config.GetValue("audio", "master_volume", 1.0f));
            ApplyVolumeDirectly(VolumeType.Music, (float)config.GetValue("audio", "music_volume", 1.0f));
            ApplyVolumeDirectly(VolumeType.Click, (float)config.GetValue("audio", "click_volume", 1.0f));
            ApplyVolumeDirectly(VolumeType.Monster, (float)config.GetValue("audio", "monster_volume", 1.0f));
            ApplyVolumeDirectly(VolumeType.Player, (float)config.GetValue("audio", "player_volume", 1.0f));
        }
        // If no settings file exists, default values (1.0) are already set in property initializers
    }

    /// <summary>
    /// Applies a volume setting directly without emitting signals or saving to disk.
    /// This is used during initialization to prevent recursive calls and unnecessary file I/O.
    /// Only updates the internal state and AudioServer, not the sliders.
    /// </summary>
    /// <param name="type">The type of volume to apply</param>
    /// <param name="value">The volume value to apply (0.0 to 1.0)</param>
    private void ApplyVolumeDirectly(VolumeType type, float value)
    {
        switch (type)
        {
            case VolumeType.Master:
                MasterVolume = value;
                AudioServer.SetBusVolumeDb(masterBusIndex, Mathf.LinearToDb(MasterVolume));
                break;
            case VolumeType.Music:
                MusicVolume = value;
                AudioServer.SetBusVolumeDb(musicBusIndex, Mathf.LinearToDb(MusicVolume));
                break;
            case VolumeType.Click:
                ClickVolume = value;
                AudioServer.SetBusVolumeDb(clickBusIndex, Mathf.LinearToDb(ClickVolume));
                break;
            case VolumeType.Monster:
                MonsterVolume = value;
                AudioServer.SetBusVolumeDb(monsterBusIndex, Mathf.LinearToDb(MonsterVolume));
                break;
            case VolumeType.Player:
                PlayerVolume = value;
                AudioServer.SetBusVolumeDb(playerBusIndex, Mathf.LinearToDb(PlayerVolume));
                break;
        }
    }
    #endregion
}

/// <summary>
/// Enumeration of available volume types that correspond to audio buses in the Godot mixer.
/// Each type represents a different category of audio that can be controlled independently.
/// 
/// Usage: Set this value in the inspector of SyncedVolumeSlider components to specify
/// which audio bus the slider should control.
/// </summary>
public enum VolumeType
{
    /// <summary>
    /// Master/General volume bus - affects all audio in the game.
    /// This is the root bus that all other buses are routed through.
    /// Use for "General Volume" or "Master Volume" sliders.
    /// </summary>
    Master,
    
    /// <summary>
    /// Music volume bus - affects background music and ambient audio.
    /// Use for "Music Volume" sliders in settings menus.
    /// </summary>
    Music,
    
    /// <summary>
    /// Click volume bus - affects UI sounds and interface feedback.
    /// Use for "UI Sounds" or "Click Volume" sliders.
    /// </summary>
    Click,
    
    /// <summary>
    /// Monster volume bus - affects enemy and creature sounds.
    /// Use for "Monster Sounds" or "Enemy Volume" sliders.
    /// </summary>
    Monster,
    
    /// <summary>
    /// Player volume bus - affects player character audio.
    /// Use for "Player Sounds" or "Character Volume" sliders.
    /// </summary>
    Player
}
