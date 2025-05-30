using Godot;

/// <summary>
/// Global volume manager for synchronizing sliders across different settings pages
/// Handles 5 audio buses: Master, Music, Click, Monster, Player
/// </summary>
public partial class VolumeManager : Node
{
    #region Signals - One signal per audio bus
    [Signal] public delegate void MasterVolumeChangedEventHandler(float value);
    [Signal] public delegate void MusicVolumeChangedEventHandler(float value);
    [Signal] public delegate void ClickVolumeChangedEventHandler(float value);
    [Signal] public delegate void MonsterVolumeChangedEventHandler(float value);
    [Signal] public delegate void PlayerVolumeChangedEventHandler(float value);
    #endregion

    #region Volume Properties
    public float MasterVolume { get; private set; } = 1.0f;
    public float MusicVolume { get; private set; } = 1.0f;
    public float ClickVolume { get; private set; } = 1.0f;
    public float MonsterVolume { get; private set; } = 1.0f;
    public float PlayerVolume { get; private set; } = 1.0f;
    #endregion

    #region Private Fields
    // Audio bus indices (matching your Godot audio setup)
    private int masterBusIndex;
    private int musicBusIndex;
    private int clickBusIndex;
    private int monsterBusIndex;
    private int playerBusIndex;
    
    private const string SETTINGS_FILE_PATH = "user://volume_settings.cfg";
    #endregion

    public override void _Ready()
    {
        // Get bus indices from your audio mixer setup
        masterBusIndex = AudioServer.GetBusIndex("Master");
        musicBusIndex = AudioServer.GetBusIndex("Music");
        clickBusIndex = AudioServer.GetBusIndex("Click");
        monsterBusIndex = AudioServer.GetBusIndex("Monster");
        playerBusIndex = AudioServer.GetBusIndex("Player");
        
        LoadVolumeSettings();
    }

    #region Public Volume Setters
    public void SetMasterVolume(float value)
    {
        MasterVolume = Mathf.Clamp(value, 0.0f, 1.0f);
        AudioServer.SetBusVolumeDb(masterBusIndex, Mathf.LinearToDb(MasterVolume));
        EmitSignal(SignalName.MasterVolumeChanged, MasterVolume);
        SaveVolumeSettings();
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = Mathf.Clamp(value, 0.0f, 1.0f);
        AudioServer.SetBusVolumeDb(musicBusIndex, Mathf.LinearToDb(MusicVolume));
        EmitSignal(SignalName.MusicVolumeChanged, MusicVolume);
        SaveVolumeSettings();
    }

    public void SetClickVolume(float value)
    {
        ClickVolume = Mathf.Clamp(value, 0.0f, 1.0f);
        AudioServer.SetBusVolumeDb(clickBusIndex, Mathf.LinearToDb(ClickVolume));
        EmitSignal(SignalName.ClickVolumeChanged, ClickVolume);
        SaveVolumeSettings();
    }

    public void SetMonsterVolume(float value)
    {
        MonsterVolume = Mathf.Clamp(value, 0.0f, 1.0f);
        AudioServer.SetBusVolumeDb(monsterBusIndex, Mathf.LinearToDb(MonsterVolume));
        EmitSignal(SignalName.MonsterVolumeChanged, MonsterVolume);
        SaveVolumeSettings();
    }

    public void SetPlayerVolume(float value)
    {
        PlayerVolume = Mathf.Clamp(value, 0.0f, 1.0f);
        AudioServer.SetBusVolumeDb(playerBusIndex, Mathf.LinearToDb(PlayerVolume));
        EmitSignal(SignalName.PlayerVolumeChanged, PlayerVolume);
        SaveVolumeSettings();
    }
    #endregion

    #region Settings Management
    private void SaveVolumeSettings()
    {
        var config = new ConfigFile();
        config.SetValue("audio", "master_volume", MasterVolume);
        config.SetValue("audio", "music_volume", MusicVolume);
        config.SetValue("audio", "click_volume", ClickVolume);
        config.SetValue("audio", "monster_volume", MonsterVolume);
        config.SetValue("audio", "player_volume", PlayerVolume);
        config.Save(SETTINGS_FILE_PATH);
    }

    private void LoadVolumeSettings()
    {
        var config = new ConfigFile();
        if (config.Load(SETTINGS_FILE_PATH) == Error.Ok)
        {
            ApplyVolumeDirectly(VolumeType.Master, (float)config.GetValue("audio", "master_volume", 1.0f));
            ApplyVolumeDirectly(VolumeType.Music, (float)config.GetValue("audio", "music_volume", 1.0f));
            ApplyVolumeDirectly(VolumeType.Click, (float)config.GetValue("audio", "click_volume", 1.0f));
            ApplyVolumeDirectly(VolumeType.Monster, (float)config.GetValue("audio", "monster_volume", 1.0f));
            ApplyVolumeDirectly(VolumeType.Player, (float)config.GetValue("audio", "player_volume", 1.0f));
        }
    }

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
/// Volume types matching your Godot audio bus setup
/// </summary>
public enum VolumeType
{
    Master,     // Master bus (General volume)
    Music,      // Music bus
    Click,      // Click sounds bus
    Monster,    // Monster sounds bus
    Player      // Player sounds bus
}
