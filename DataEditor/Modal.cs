using System;
using Engine;

public class Modal
{
    static readonly Modal instance = new Modal();

    string msg = "No Message";
    string true_lbl = "Yes";
    string false_lbl = "No";
    Action true_act;
    Action false_act;

    bool show;

    Modal() { }

    public static void Draw() => instance.draw();

    public static void ShowDialog(string msg, Action true_act, Action false_act, string true_label = "Yes", string false_label = "No")
    {
        instance.msg = msg;
        instance.true_lbl = true_label;
        instance.false_lbl = false_label;
        instance.true_act = true_act;
        instance.false_act = false_act;
        instance.show = true;
    }

    void draw()
    {
        var gui = App.Gui;
        if (show)
        {
            gui.OpenPopup("ModalDialog");
            show = false;
        }

        if (gui.BeginPopupModal("ModalDialog", WindowFlags.AlwaysAutoResize | WindowFlags.NoTitleBar))
        {
            gui.Text(msg);
            gui.Separator();

            if (gui.Button(true_lbl))
            {
                true_act?.Invoke();
                true_act = false_act = null;
                gui.CloseCurrentPopup();
            }
            gui.SameLine();
            if (gui.Button(false_lbl))
            {
                false_act?.Invoke();
                true_act = false_act = null;
                gui.CloseCurrentPopup();
            }

            gui.EndPopup();
        }
    }
}