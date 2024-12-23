namespace advent;
public class Coords
{
    public Coords(long x, long y)
    {
        X = x;
        Y = y;
    }

    public long X { get; set; }
    public long Y { get; set; }

    public bool Equals(Coords other)
    {
        if (Object.ReferenceEquals(other, null)) return false;
        if (Object.ReferenceEquals(this, other)) return true;
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }
}