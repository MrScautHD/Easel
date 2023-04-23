﻿using System.Reflection;
#if !HEADLESS
using Easel.Graphics;
using Easel.Graphics.Renderers;
using Easel.Math;
using Pie;
using Pie.Windowing;
#endif

namespace Easel;

/// <summary>
/// Initial game settings used on game startup.
/// </summary>
public struct GameSettings
{
#if !HEADLESS
    /// <summary>
    /// The starting size (resolution) of the game view, in pixels. (Default: 1280x720)
    /// </summary>
    public Size<int> Size;

    /// <summary>
    /// If enabled, the window will start in fullscreen mode at the given <see cref="Size"/>. (Default: false)
    /// </summary>
    public bool Fullscreen;

    /// <summary>
    /// The starting title of the game view. (Default: The starting assembly name)
    /// </summary>
    public string Title;

    /// <summary>
    /// Whether or not the game will synchronize to the vertical refresh rate. (Default: <see langword="true" />)
    /// </summary>
    public bool VSync;
    
    /// <summary>
    /// If enabled, Easel will not error if it tries to load items that do not exist, such as textures, instead
    /// displaying a default "missing" object. (Default: <see langword="false" />
    /// </summary>
    public bool AllowMissing;

    /// <summary>
    /// The title bar flags, if any (Default: <see cref="Easel.TitleBarFlags.ShowEasel"/>)
    /// </summary>
    public TitleBarFlags TitleBarFlags;

    /// <summary>
    /// If disabled, the game window will not be visible until you tell it to become visible.
    /// </summary>
    public bool StartVisible;
    
    /// <summary>
    /// The initial border of the view. (Default: <see cref="WindowBorder.Fixed"/>)
    /// </summary>
    public WindowBorder Border;
    
    /// <summary>
    /// The graphics API the game will use - leave null to let Easel decide which API to use. (Default: <see langword="null"/>)
    ///
    /// Default API:
    /// <list type="bullet">
    ///     <item><b>Windows -</b> <see cref="GraphicsApi.D3D11"/></item>
    ///     <item><b>Linux -</b> <see cref="GraphicsApi.OpenGl33"/></item>
    ///     <item><b>macOS -</b> <see cref="GraphicsApi.OpenGl33"/> - Warning: macOS is not officially supported</item>
    /// </list>
    /// </summary>
    public GraphicsApi? Api;
    
    /// <summary>
    /// The view icon, if any. (Default: <see langword="null" />)
    /// </summary>
    public Bitmap Icon;

    /// <summary>
    /// The render options Easel will use for your application.
    /// </summary>
    public RenderOptions RenderOptions;

    /// <summary>
    /// If <b>NOT</b> null, a content definition will be automatically generated. You should only use this during
    /// development. When publishing, you should create a content file. (Default: "Content" in Debug, null in any other
    /// mode).
    /// </summary>
    public string AutoGenerateContentDirectory;

#endif

    /// <summary>
    /// The target frames per second of the game. Set to 0 to have unlimited FPS. (Default: 0, if <see cref="VSync"/> is
    /// enabled the game will run at the monitor's native refresh rate (typically 60, 144, etc.)
    /// </summary>
    public int TargetFps;

#if !HEADLESS
    public GameSettings(Size<int> size, bool fullscreen, string title, bool vSync, bool allowMissing,
        TitleBarFlags titleBarFlags, bool startVisible, WindowBorder border, GraphicsApi? api, Bitmap icon,
        RenderOptions renderOptions, string autoGenerateContentDirectory, int targetFps)
    {
        Size = size;
        Fullscreen = fullscreen;
        Title = title;
        VSync = vSync;
        AllowMissing = allowMissing;
        TitleBarFlags = titleBarFlags;
        StartVisible = startVisible;
        Border = border;
        Api = api;
        Icon = icon;
        RenderOptions = renderOptions;
        AutoGenerateContentDirectory = autoGenerateContentDirectory;
        TargetFps = targetFps;
    }
    
    /// <summary>
    /// Create the default game settings.
    /// </summary>
    public GameSettings()
    {
        Size = new Size<int>(1280, 720);
        Fullscreen = false;

        Title = Assembly.GetEntryAssembly()?.GetName().Name ?? "Easel Window";
        Border = WindowBorder.Fixed;
        VSync = true;
        TargetFps = 0;
        Api = null;
        AllowMissing = false;
        Icon = null;
        TitleBarFlags = TitleBarFlags.ShowEasel;
        StartVisible = true;
        RenderOptions = RenderOptions.Default;
#if DEBUG
        AutoGenerateContentDirectory = "Content";
#else
        AutoGenerateContentDirectory = null;
#endif
    }

    public static GameSettings StartFullscreen => new GameSettings()
    {
        Size = new Size<int>(-1, -1),
        Fullscreen = true
    };
#else
    public GameSettings(int targetFps)
    {
        TargetFps = targetFps;
    }
    
    /// <summary>
    /// Create the default game settings.
    /// </summary>
    public GameSettings()
    {
        TargetFps = 0;
    }
#endif
}