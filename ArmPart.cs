using Godot;
using System;

namespace Robo;

public partial class ArmPart : Part
{
    public bool ReadyState => _readyState;
    
    private bool _readyState = false;

    public void ReadyArm()
    {
        _readyState = !_readyState;
        
        if (_readyState)
        {
            var rotation = this.Rotation;
            rotation.X = Mathf.DegToRad(90);
            this.Rotation = rotation;
        }

        if (!_readyState)
        {
            var rotation = this.Rotation;
            rotation.X = Mathf.DegToRad(0);
            this.Rotation = rotation;
        }
        
    }
}

