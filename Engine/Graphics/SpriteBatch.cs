using System;
using System.Numerics;
using System.Collections.Generic;
using static System.MathF;

namespace Engine
{
    public class SpriteBatch : DrawableMesh
    {
        static IComparer<SpriteNode> sort_by_order = Comparer<SpriteNode>.Create((SpriteNode x, SpriteNode y) => x.OrderInBuffer - y.OrderInBuffer);

        Array<SpriteNode> nodes;

        public SpriteBatch()
        {
            nodes = new Array<SpriteNode>();
        }

        /// <summary>
        /// Add sprite node to the batch
        /// Note: You need to call rebuild after this to apply changes
        /// </summary>
        public void AddSprite(SpriteNode node) => nodes.Push(node);

        /// <summary>
        /// Removes sprite node from the batch
        /// Note: You need to call rebuild after this to apply changes
        /// </summary>
        public void RemoveSprite(SpriteNode node) => nodes.Remove(node);

        /// <summary>
        /// Clears batch from all sprite nodes
        /// </summary>
        public void Clear() => nodes.Clear();

        /// <summary>
        /// Rebuilds batch from the sprite nodes
        /// </summary>
        public void Rebuild()
        {
            nodes.Sort(sort_by_order);
            VertexArray.Clear();
            IndexArray.Clear();
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var pos = nodes[i].LocalToWorld(nodes[i].Frame.Vertices[j].Position);
                    VertexArray.Push(new Vertex(pos, nodes[i].Frame.Vertices[j].Texcoord, nodes[i].Color));
                }
                var offset = (ushort)(i * 4);
                IndexArray.Push((ushort)(offset + 0));
                IndexArray.Push((ushort)(offset + 1));
                IndexArray.Push((ushort)(offset + 2));
                IndexArray.Push((ushort)(offset + 0));
                IndexArray.Push((ushort)(offset + 1));
                IndexArray.Push((ushort)(offset + 3));
            }
            UpdateAll();
        }

        protected override void OnDisposeManaged()
        {
            base.OnDisposeManaged();
            nodes.Clear();
            nodes = null;
        }
    }
}
