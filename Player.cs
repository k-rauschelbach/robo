using Godot;
using System;

namespace Robo;

public enum PlayerState
{
    Explore,
    Combat
}

public partial class Player : CharacterBody3D
{
    [Export] public float MoveSpeed { get; set; } = 5.0f;
    [Export] public float Gravity { get; set; } = 9.8f;
    [Export] public float MouseSensitivity { get; set; } = 0.002f;
    

    private Part _head;
    private Part _body;
    private ArmPart _rightArm;
    private ArmPart _leftArm;
    
    private Camera3D _camera;
    private Node3D _cameraArm;
    private bool _combatState;
    private PlayerState _playerState;
    
    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("CameraArm/Camera3D");
        _cameraArm = GetNode<Node3D>("CameraArm");
        _head = GetNode<Part>("Head");
        _body = GetNode<Part>("Body");
        _rightArm = GetNode<ArmPart>("RightArm");
        _leftArm = GetNode<ArmPart>("LeftArm");
        
        _playerState = PlayerState.Explore;
        
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            if (!IsReadied)
            {
                RotateY(-mouseMotion.Relative.X * MouseSensitivity);
            
                var rotation = _cameraArm.Rotation;
                rotation.X += -mouseMotion.Relative.Y * MouseSensitivity;
                rotation.X = Mathf.Clamp(rotation.X, -0.8f, 0.4f);
                _cameraArm.Rotation = rotation;
            }

        }

        if (@event.IsActionPressed("ui_cancel"))
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }

        if (@event is InputEventMouseButton mouseButton && mouseButton.IsPressed())
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            
            if (_playerState == PlayerState.Explore)
                SetPlayerState(PlayerState.Combat);
        }

        if (@event.IsActionPressed("sheath"))
        {
            if (_playerState == PlayerState.Combat)
                SetPlayerState(PlayerState.Explore);
            else
            {
                SetPlayerState(PlayerState.Combat);
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        if (!IsOnFloor())
        {
            velocity.Y = -Gravity * (float)delta;
        }

        Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * MoveSpeed;
            velocity.Z = direction.Z * MoveSpeed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(velocity.X, 0, MoveSpeed);
            velocity.Z = Mathf.MoveToward(velocity.Z, 0, MoveSpeed);
        }
        
        Velocity = velocity;
        MoveAndSlide();
    }

    private void SetPlayerState(PlayerState newState)
    {
        if (newState == _playerState)
            return;
        
        switch (newState)
        {
            case PlayerState.Combat:
                GetNode<ArmPart>("RightArm").ReadyArm();
                GetNode<CameraArm>("CameraArm").CombatCamera();
                _playerState = newState;
                break;
            case PlayerState.Explore:
                GetNode<ArmPart>("RightArm").UnReadyArm();
                GetNode<CameraArm>("CameraArm").ExploreCamera();
                _playerState = newState;
                break;
        }
    }
}
