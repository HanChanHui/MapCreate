using System;

public class TilePosition : IEquatable<TilePosition>
{
    public int x;
    public int z;

    public TilePosition(int _x, int _z)
    {
        this.x = _x;
        this.z = _z;
    }

    public override bool Equals(object obj)
    {
        return obj is TilePosition position &&
                x == position.x &&
                z == position.z;
    }

    public bool Equals(TilePosition _other)
    {
        return this == _other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public static bool operator ==(TilePosition a, TilePosition b)
    {
        return a.x == b.x && a.z == b.z;
    }

    public static bool operator !=(TilePosition a, TilePosition b)
    {
        return !(a == b);
    }

    public static TilePosition operator +(TilePosition a, TilePosition b)
    {
        return new TilePosition(a.x + b.x, a.z + b.z);
    }

    public static TilePosition operator -(TilePosition a, TilePosition b)
    {
        return new TilePosition(a.x - b.x, a.z - b.z);
    }
}
