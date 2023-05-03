using System;

public class GameObjectNotFoundException : Exception
{
    public GameObjectNotFoundException(string message) : base(message)
    {
    }
}