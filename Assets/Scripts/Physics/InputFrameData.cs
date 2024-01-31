using Godot;

public static class InputFrameData{

    #region Direction
    public static Vector2 Direction{get=>Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");}
    public static float AxisX{get=>Input.GetAxis("ui_left", "ui_right");}
    #endregion
    
    #region Jump
    public static bool JustPressedJump{get=>Input.IsActionJustPressed("ui_accept");}
    public static bool PressingJump{get=>Input.IsActionPressed("ui_accept");}
    public static bool ReleaseJump{get=>Input.IsActionJustReleased("ui_accept");}
    #endregion

    #region Down
    public static bool JustPressedDown{get=>Input.IsActionJustPressed("ui_down");}
    public static bool PressingDown{get=>Input.IsActionPressed("ui_down");}
    public static bool ReleaseDown{get=>Input.IsActionJustReleased("ui_down");}
    #endregion

}