public enum HexDirection
{
    N, NE, SE, S, SW, NW
}

public static class HexDirectionExtensions
{
    // return the opposite direction
    public static HexDirection Opposite(this HexDirection dir)
    {
        return (int)dir < 3 ? (dir + 3) : (dir - 3);
    }

    // return previous direction
    public static HexDirection Previous(this HexDirection dir)
    {
        return dir == HexDirection.N ? HexDirection.NW : (dir - 1);
    }

    // return previous direction
    public static HexDirection Next(this HexDirection dir)
    {
        return dir == HexDirection.NW ? HexDirection.N : (dir + 1);
    }
}
