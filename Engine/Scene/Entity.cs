using System;
using System.Numerics;

// namespace Engine
// {
//     public class Entity : ObjectBase
//     {
//         Entity next;
//         Entity prev;
//         Node root;
//         Scene scene;
// 
//         public Scene Scene => scene;
// 
//         public Node RootNode => root;
// 
//         internal override void InternalDestroy()
//         {
//             OnDestroy();
//             if (Scene.InternalGetFirstNode() == this)
//                 Scene.InternalSetFirstNode(next);
// 
//             if (prev != null)
//                 prev.next = next;
// 
//             if (next != null)
//                 next.prev = prev;
// 
//             root.Destroy();
//         }
// 
//         public T AddNode<T>() where T : Node, new()
//         {
//             var node = new T();
//             node.Name = typeof(T).Name;
//             if (root == null)
//             {
//                 root = node;
//                 node.InternalBegin(this, null);
//             }
//             else
//             {
//                 node.InternalBegin(this, root);
//             }
// 
//             return node;
//         }
// 
//         protected virtual void OnUpdate(float dt) { }
// 
//         internal void InternalBegin(Scene scene, string name, Entity first)
//         {
//             this.scene = scene;
//             this.Name = name;
//             if (first != null)
//                 first.prev = this;
// 
//             this.next = first;
//             OnBegin();
//         }
// 
//         internal void InternalUpdate(float dt) => OnUpdate(dt);
// 
//         internal Entity InternalGetNext() => next;
// 
//         internal void InternalSetRootNode(Node node) => root = node;
//     }
// }