using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public class Scene : ObjectBase
    {
        Dictionary<string, Array<Node>> groups;
        Array<Node> rootNodes;
        CameraNode mainCamera;

        public CameraNode MainCamera { get; set; }

        public Scene()
        {
            groups = new Dictionary<string, Array<Node>>();

            // default groups
            groups.Add("Node.Draw", new Array<Node>());
            groups.Add("Node.Update", new Array<Node>());
            groups.Add("Node.Destroy", new Array<Node>());

            rootNodes = new Array<Node>();
        }

        public virtual void OnBecameActive() { }

        public Node Find(string path)
        {
            Node searchInRoot(string name)
            {
                int hash = Hash.FromString(name);
                for (int i = 0; i < rootNodes.Count; i++)
                {
                    if (rootNodes[i].CompareName(name, hash))
                        return rootNodes[i];
                }

                return null;
            }

            if (path.Contains("."))
            {
                var tokens = path.Split('.');

                Node node = searchInRoot(tokens[0]);
                for (int i = 1; i < tokens.Length; i++)
                {
                    if (node == null)
                        return null;

                    node = node.Find(tokens[i]);
                }

                return node;
            }
            else
            {
                return searchInRoot(path);
            }
        }

        public T Spawn<T>(string name) where T : Node, new()
        {
            var node = new T();
            node.Name = name;
            node.InternalBegin(this, null);
            rootNodes.Push(node);
            return node;
        }

        public IReadOnlyArray<Node> GetNodesByGroup(string name)
        {
            groups.TryGetValue(name, out Array<Node> array);
            return array;
        }

        protected virtual void OnUpdate(float dt) { }

        internal void NewFrame()
        {
            var dt = Time.FrameTime;
            OnUpdate(dt);

            var updates = GetNodesByGroup("Node.Update");
            for (int i = 0; i < updates.Count; i++)
            {
                var node = updates.GetItemAt(i) as IUpdateNode;
                node.Update(dt);
            }

            var destroys = GetNodesByGroup("Node.Destroy") as Array<Node>;
            for (int i = 0; i < destroys.Count; i++)
                destroys[i].DestroyImp(true);

            (destroys as Array<Node>).Clear();
        }

        internal void RegisterNode(string group, Node node)
        {
            if (!groups.TryGetValue(group, out Array<Node> array))
                groups[group] = array = new Array<Node>();

            array.Push(node);
        }

        internal void UnregisterNode(string group, Node node)
        {
            if (groups.TryGetValue(group, out Array<Node> array))
                array.Remove(node);
        }

        internal override void DestroyImp(bool destroying)
        {
            if (!IsDestroyed)
            {
                OnDestroy();

                for (int i = rootNodes.Count - 1; i >= 0; --i)
                    Destroy(rootNodes[i]);

                IsDestroyed = true;
                GC.SuppressFinalize(this);
            }
        }

        internal void RemoveNode(Node node)
        {
            rootNodes.SwapAndPop(node);
        }
    }
}