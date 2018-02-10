using System;
using System.Numerics;
using System.Collections.Generic;

namespace Engine
{
    public sealed partial class Gui
    {
        public void Gradient(string label, Gradient grad)
        {
            PushID(grad.GetHashCode());
            var wpos = GetWindowPosition();
            var gpos = GetCursorScreenPos();
            var grect = new Rect(gpos.X, gpos.Y, GetWindowWidth() * .65f, 30);

            // draws checker
            AddImage(checker, grect, new Rect(Vector2.Zero, grect.Size / 20f), Color.White);

            // draws gradient colors
            for (int i = 0; i < grad.KeyCount - 1; i++)
            {
                var color1 = grad.GetColor(i);
                var color2 = grad.GetColor(i + 1);

                var gx = grad.GetPosition(i);
                var gw = grad.GetPosition(i + 1) - gx;

                AddRectFilledMultiColor(new Rect(grect.X + gx * grect.Width, grect.Y, gw * grect.Width, grect.Height), color1, color2, color2, color1);
            }

            // draws gradient line
            AddLine(new Vector2(grect.XMin, grect.YHalf), new Vector2(grect.XMax, grect.YHalf), Color.White, 1);

            int del_index = -1;
            for (int i = 0; i < grad.KeyCount; i++)
            {
                // draw keys
                var col_gpos = new Vector2(gpos.X + grad.GetPosition(i) * grect.Width, gpos.Y + grect.Height * .5f);

                AddCircleFilled(col_gpos, 4, Color.White, 16);
                AddCircleFilled(col_gpos, 2.75f, grad.GetColor(i), 16);

                SetCursorScreenPos(col_gpos - Vector2.One * 10);

                // invisible button used to move keys
                InvisibleButton($"col_{grad.GetId(i)}", Vector2.One * 20);
                if (IsItemActive())
                {
                    if (i != 0 && i != grad.KeyCount - 1)
                    {
                        var key_pos = ((GetMousePos().X - gpos.X) / grect.Width).Clamp(0.05f, 0.95f);
                        grad.SetPosition(i, key_pos);
                    }
                }

                // context menu for remove or edit color of keys
                if (BeginPopupContextItem($"key_context{i}"))
                {
                    var key_color = grad.GetColor(i);
                    ColorPicker(string.Empty, ref key_color, ColorEditFlags.NoSidePreview);
                    grad.SetColor(i, key_color);

                    if (i > 0 && i < grad.KeyCount - 1 && Button("Remove", new Vector2(-1, 20)))
                    {
                        del_index = i;
                        CloseCurrentPopup();
                    }

                    EndPopup();
                }
            }

            // remove the specified key
            if (del_index > 0)
                grad.RemoveKey(del_index);

            // invisible button for adding new key
            SetCursorScreenPos(gpos);
            if (InvisibleButton("add_key", grect.Size))
            {
                var new_key_pos = ((GetMousePos().X - gpos.X) / grect.Width).Clamp(0.05f, 0.95f); ;
                grad.AddKey(new_key_pos);
            }
            // gradient label
            SameLine(0, 4);
            Text(label);

            PopID();
        }
    }
}