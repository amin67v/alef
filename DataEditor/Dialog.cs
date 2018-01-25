using System;
using System.Numerics;

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
    Vector2 btn_size = new Vector2(80, 0);

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

    public static void Info(string msg)
    {
        Instance.msg = msg;
        Instance.true_act = null;
        Instance.false_act = null;
        Instance.ask_type = Instance.info;
        Instance.set_to_show = true;
    }

    public static void UserInput(string msg, Action ok, Action cancel, string text = "")
    {
        Instance.text.String = text;
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
            if (Input.IsKeyPressed(KeyCode.Enter) ||Input.IsKeyPressed(KeyCode.KpEnter))
            {
                true_act?.Invoke();
                close(gui);
            }
            else if (Input.IsKeyPressed(KeyCode.Escape))
            {
                false_act?.Invoke();
                close(gui);
            }
            gui.EndPopup();
        }
    }

    void yes_or_no(Gui gui)
    {
        gui.Text(msg);
        gui.Separator();

        if (gui.Button("Yes", btn_size))
        {
            true_act?.Invoke();
            close(gui);
        }
        gui.SameLine();
        if (gui.Button("No", btn_size))
        {
            false_act?.Invoke();
            close(gui);
        }
    }

    void info(Gui gui)
    {
        gui.Text(msg);
        gui.Separator();

        if (gui.Button("Ok", btn_size))
            close(gui);
    }

    void user_text(Gui gui)
    {
        gui.Text(msg);
        gui.InputText(string.Empty, text, InputTextFlags.Default);
        gui.Separator();

        if (gui.Button("Ok", btn_size))
        {
            true_act?.Invoke();
            close(gui);
        }
        gui.SameLine();
        if (gui.Button("Cancel", btn_size))
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