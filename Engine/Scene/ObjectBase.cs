// namespace Engine
// {
//     public abstract class ObjectBase
//     {
//         static Log log;
//         static Input input;
// 
//         string name;
//         int hashName;
//         bool isDestroyed;
// 
//         public static Log Log 
//         {
//             get
//             {
//                 if (log == null || log.IsDisposed)
//                     log = App.Get<Log>();
//                 
//                 return log;
//             }
//         }
// 
//         public static Input Input
//         {
//             get
//             {
//                 if (input == null || input.IsDisposed)
//                     input = App.Get<Input>();
// 
//                 return input;
//             }
//         }
// 
//         /// <summary>
//         /// Is this object already destroyed
//         /// </summary>
//         public bool IsDestroyed => isDestroyed;
// 
//         /// <summary>
//         /// Gets or sets name of the gameObject
//         /// </summary>
//         public string Name
//         {
//             get => name;
//             set
//             {
//                 name = value;
//                 hashName = Hash.FromString(name);
//             }
//         }
// 
//         public void Destroy()
//         {
//             if (!isDestroyed)
//             {
//                 InternalDestroy();
//                 isDestroyed = true;
//             }
//             else
//             {
//                 App.Log.Warning($"Multiple destroy called on object with name '{Name}'");
//             }
//         }
// 
//         /// <summary>
//         /// Called as soon as object is created
//         /// </summary>
//         protected virtual void OnBegin() { }
// 
//         /// <summary>
//         /// Called as soon as object is going to be destroyed
//         /// </summary>
//         protected virtual void OnDestroy() { }
// 
//         internal virtual void InternalDestroy() { }
// 
//         internal int InternalGetHashName() => hashName;
//     }
// }