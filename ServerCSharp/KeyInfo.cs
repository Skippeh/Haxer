namespace ServerCSharp
{
    public struct KeyInfo
    {
        public bool Shift;
        public bool Ctrl;
        public bool Alt;
        public char Character;

        public KeyInfo(bool shift, bool ctrl, bool alt, char character)
        {
            Shift = shift;
            Ctrl = ctrl;
            Alt = alt;
            Character = character;
        }
    }
}