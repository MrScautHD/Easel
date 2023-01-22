﻿using Easel;
using Easel.Tests;
using Easel.Tests.TestScenes;
using Pie.Windowing;

GameSettings settings = new GameSettings()
{
    Border = WindowBorder.Resizable
};

Logger.UseConsoleLogs();

using TestGame game = new TestGame(settings, new TestCanvas());
game.Run();