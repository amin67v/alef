//using System;
//using Engine;
//using ContextItem = Engine.Pair<string, System.Action>;
//
//public class ContextMenu
//{
//    public static readonly ContextMenu Instance = new ContextMenu();
//
//    Array<ContextItem> items = new Array<ContextItem>();
//    bool show, begin;
//
//    ContextMenu() { }
//
//    public static void Begin()
//    {
//        Instance.begin = true;
//        Instance.items.Clear(true);
//    }
//
//    public static void AddItem(string name, Action action)
//    {
//        if (Instance.begin == false)
//            throw new Exception("You need to call 'Begin' before AddItem.");
//
//        Instance.items.Push(new ContextItem(name, action));
//    }
//
//    public static void Show()
//    {
//        if (Instance.begin == false)
//            throw new Exception("You need to call 'Begin' before Show.");
//
//        Instance.begin = false;
//        Instance.show = true;
//    }
//
//    public void Draw(Gui gui)
//    {
//        if (show)
//        {
//            gui.OpenPopup("context_menu");
//            show = false;
//        }
//
//        if (gui.BeginPopup("context_menu"))
//        {
//            for (int i = 0; i < items.Count; i++)
//                if (gui.Selectable(items[i].Key))
//                    items[i].Value?.Invoke();
//
//            gui.EndPopup();
//        }
//
//    }
//}