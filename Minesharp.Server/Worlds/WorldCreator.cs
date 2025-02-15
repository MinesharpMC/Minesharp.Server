using Minesharp.Server.Chunks.Generator;

namespace Minesharp.Server.Worlds;

public class WorldCreator
{
    public string Name { get; init; }
    public ChunkGenerator ChunkGenerator { get; init; }
    public Difficulty Difficulty { get; init; }
    public GameMode GameMode { get; init; }
    public Position SpawnPosition { get; init; }
    public Rotation SpawnRotation { get; init; }
}