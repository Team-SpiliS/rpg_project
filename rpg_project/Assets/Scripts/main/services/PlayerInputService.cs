using System;

public class PlayerInputService : IPlayerInputService, IDisposable
{
    public PlayerControls Controls { get; }

    public PlayerInputService()
    {
        Controls = new PlayerControls();
        Controls.Enable();
    }

    public void Dispose()
    {
        Controls.Disable();
    }
}