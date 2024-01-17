using Godot;

public partial class CustomSignals : Node{

[Signal]
public delegate void StateChangedSignalEventHandler(State newState, State previousState);
}