using System.Collections.Concurrent;
using Minesharp.Common;
using Minesharp.Common.Enum;
using Minesharp.Game.Blocks;
using Minesharp.Game.Chunks;
using Minesharp.Game.Entities;
using Minesharp.Packet;

namespace Minesharp.Game.Worlds;

public sealed class World
{
    private readonly ChunkFactory chunkFactory;
    private readonly ConcurrentDictionary<ChunkKey, Chunk> chunks;

    public World(WorldCreator creator)
    {
        Id = Guid.NewGuid();
        Name = creator.Name;
        Border = creator.Border;
        Difficulty = creator.Difficulty;
        GameMode = creator.GameMode;

        chunkFactory = new ChunkFactory(creator.ChunkGenerator, this);
        chunks = new ConcurrentDictionary<ChunkKey, Chunk>();
    }

    public Guid Id { get; }
    public string Name { get; }
    public WorldBorder Border { get; }
    public Difficulty Difficulty { get; }
    public GameMode GameMode { get; }
    public List<Player> Players { get; } = new();
    
    public Block GetBlockAt(int x, int y, int z)
    {
        return new Block
        {
            World = this,
            X = x,
            Y = y,
            Z = z
        };
    }

    public Material GetBlockTypeAt(Position position)
    {
        return GetBlockTypeAt(position.BlockX, position.BlockY, position.BlockZ);
    }

    public void SetBlockTypeAt(Position position, Material material)
    {
        SetBlockTypeAt(position.BlockX, position.BlockY, position.BlockZ, material);
    }
    
    public void SetBlockTypeAt(int x, int y, int z, Material material)
    {
        var chunk = GetChunkAt(x >> 4, z >> 4);
        chunk.SetTypeAt(x, y, z, material);
    }

    public Material GetBlockTypeAt(int x, int y, int z)
    {
        var chunk = GetChunkAt(x >> 4, z >> 4);
        return chunk.GetTypeAt(x & 0xf, y, z & 0xf);
    }

    public Block GetBlockAt(Position position)
    {
        return GetBlockAt(position.BlockX, position.BlockY, position.BlockZ);
    }
    
    public Chunk GetChunkAt(Position position)
    {
        return GetChunkAt(position.BlockX, position.BlockZ);
    }

    public Chunk GetChunkAt(int x, int z)
    {
        return GetChunk(ChunkKey.Of(x >> 4, z >> 4));
    }
    
    public Chunk GetChunk(ChunkKey key)
    {
        var chunk = chunks.GetValueOrDefault(key);
        if (chunk is null)
        {
            chunks[key] = chunk = chunkFactory.Create(key);
        }

        return chunk;
    }

    public IEnumerable<Chunk> GetChunks()
    {
        return chunks.Values;
    }

    public void Tick()
    {
        foreach (var player in Players)
        {
            player.Tick();
        }

        foreach (var chunk in chunks.Values)
        {
            chunk.Tick();
        }
    }
}