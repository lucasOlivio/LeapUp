using System;

namespace CustomExceptions
{
    public class GameObjectNotFoundException : Exception
    {
        public GameObjectNotFoundException(string message) : base(message)
        {
        }
    }
}