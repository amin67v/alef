
public abstract class EditableData
{
    public abstract EditableType Type { get; }
}

public enum EditableType
{
    SpriteSheet,
    Texture,
    Shape,
    Particle,
    Entity
}