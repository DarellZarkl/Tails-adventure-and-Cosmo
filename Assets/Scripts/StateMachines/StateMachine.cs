using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Godot;



public partial class StateMachine : Node{
    [Signal]
    public delegate void StateChangedSignalEventHandler(State state);
    public State CurrentState{get; private set;}
    public LinkedList<State> PreviousStates{get;set;} = new LinkedList<State>();
    public List<State> AvailableStates{get;set;}

    [Export]
    public NodePath DefaultStatePath{get;set;}
    public State DefaultState{get;set;}

    public override void _Ready()
    {
        base._Ready();
        DefaultState = GetNode<State>(DefaultStatePath);
        GD.Print("state : "+DefaultState);
        GD.Print("state : Updating...");
        this.UpdateState(DefaultState);
        GD.Print("state : Updated...");
    }

    public virtual void UpdateState(State nextState){
        if(CurrentState==nextState){
            return;
        }
        if(CurrentState!=null){
            PreviousStates.AddFirst(CurrentState);
            CurrentState.EndState();
        }
        CurrentState= nextState;
        if(nextState!=null){
            EmitSignal(nameof(StateChangedSignal),CurrentState);
            CurrentState.StartState();
        }
    }
    public virtual void ExecuteStateLogic(){
        if(CurrentState!=null){
            this.CurrentState.Execute();
            //GD.Print("Executed state logic : "+this.CurrentState?.GetType()?.Name);
        }
    }
    public virtual State? GetPreviousStateByIndex(int index=0){
        if(PreviousStates.Count>=index+1){
            return PreviousStates.ToList()[index];
        }
        else{
            return null;
        }
    }
   
}
public partial class State : Node{
    public override void _Ready()
    {
        base._Ready();
        GD.Print(this.GetType().Name);
    }
    public virtual void StartState(){

    }
    public virtual void EndState(){
        
    }
    public virtual void Execute(){

    }
     public virtual bool EqualType(Type state){
        var type = this.GetType();
        return type == state;
     }
}