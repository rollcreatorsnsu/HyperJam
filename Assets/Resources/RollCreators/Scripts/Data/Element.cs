using System;

[Serializable]
public class Element
{
    public ElementType type;
    public int connection;
    public int rotation;
    [NonSerialized] public bool connected;
    [NonSerialized] public int resistorLives = 0;

    public Element()
    {
        type = ElementType.NONE;
    }

    public void Turn()
    {
        connection <<= 1;
        connection |= connection >> 4;
        connection &= 15;
        rotation++;
        rotation %= 4;
    }
}