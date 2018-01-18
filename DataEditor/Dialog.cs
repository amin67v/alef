using System;
using Engine;

public class Dialog
{
    public static readonly Dialog Instance = new Dialog();

    string msg = "No Message";
    Action true_act;
    Action false_act;
    Action<Gui> ask_type;
    InputBuffer text = new InputBuffer(512);
    bool set_to_show;
    bool is_showing;

    Dialog() { }

    public static bool IsShowing => Instance.is_showing;

    public static string UserText => Instance.text.String;

    public static void YesOrNo(string msg, Action yes, Action no)
    {
        Instance.msg = msg;
        Instance.true_act = yes;
        Instance.false_act = no;
        Instance.ask_type = Instance.yes_or_no;
        Instance.set_to_show = true;
    }

    public static void Ok(string msg, Action ok)
    {
        Instance.msg = msg;
        Instance.true_act = ok;
        Instance.false_act = null;
        Instance.ask_type = Instance.ok;
        Instance.set_to_show = true;
    }

    public static void UserInput(string msg, Action ok, Action cancel)
    {
        Instance.msg = msg;
        Instance.true_act = ok;
        Instance.false_act = cancel;
        Instance.ask_type = Instance.user_text;
        Instance.set_to_show = true;
    }

    public void Draw(Gui gui)
    {
        if (set_to_show)
        {
            gui.OpenPopup("ModalDialog");
            is_showing = true;
            set_to_show = false;
        }

        if (gui.BeginPopupModal("ModalDialog", WindowFlags.AlwaysAutoResize | WindowFlags.NoTitleBar | WindowFlags.NoMove))
        {
            ask_type(gui);
            gui.EndPopup();
        }
    }

    void yes_or_no(Gui gui)
    {
        gui.Text(msg);
        gui.Separator();

        if (gui.Button("Yes"))
        {
            true_act?.Invoke();
            close(gui);
        }
        gui.SameLine();
        if (gui.Button("No"))
        {
            false_act?.Invoke();
            close(gui);
        }
    }

    void ok(Gui gui)
    {
        gui.Text(msg);
        gui.Separator();

        if (gui.Button("Ok"))
        {
            true_act?.Invoke();
            close(gui);
        }
    }

    void user_text(Gui gui)
    {
        gui.Text(msg);
        gui.InputText(string.Empty, text, InputTextFlags.Default);
        gui.Separator();

        if (gui.Button("Ok"))
        {
            true_act?.Invoke();
            close(gui);
        }
        gui.SameLine();
        if (gui.Button("Cancel"))
        {
            false_act?.Invoke();
            close(gui);
        }
    }

    void close(Gui gui)
    {
        gui.CloseCurrentPopup();
        is_showing = false;
        true_act = false_act = null;
    }
}