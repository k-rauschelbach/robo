using Godot;
using System;

namespace Robo;

public partial class ArmPart : Part
{
    public bool ReadyState => _readyState;
    
    private bool _readyState = false;

    public void ReadyArm()
    {
        var rotation = this.Rotation;
        rotation.X = 90;
        this.Rotation = rotation;
    }

    public void UnReadyArm()
    {
        var rotation = this.Rotation;
        rotation.X = 0;
        this.Rotation = rotation;
    }
}

