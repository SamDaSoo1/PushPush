public enum BlockType 
{ 
    Player, 
    Ball, 
    Home,
    Destroyed_Home,
    Wall, 
    Floor,
    None
}

public enum PreviousLocation
{
    Up,
    Down,
    Left,
    Right,
    None
}

public enum PreviousSprite
{
    Normal,
    Destroy
}

public enum MoveType
{
    Past,
    Current,
    None
}

public enum ButtonType
{
    UpArrow,
    DownArrow,
    LeftArrow,
    RightArrow,
    TimeLeap
}