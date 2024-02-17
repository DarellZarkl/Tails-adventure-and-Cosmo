using System.Collections.Generic;

public class GameLogicController{
    private GameLogicController _instance;
    public GameLogicController Instance{
        get{
            if(_instance==null){
                _instance = new GameLogicController();
            }
            return _instance;
        }
    }
    private GameLogicController(){}

    private CharacterController CurrentPlayerController{get;set;}
    private List<CharacterController> AllControllableCharacters{get;set;}
    public void SwitchPlayer(CharacterController controller){
        this.CurrentPlayerController = controller;
    }
    public void RegisterCharacter(CharacterController characterController){
        AllControllableCharacters.Add(characterController);
    }
    public void UnregisterCharacter(CharacterController characterController){
        AllControllableCharacters.Remove(characterController);
    }

}