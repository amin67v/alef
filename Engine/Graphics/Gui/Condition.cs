﻿namespace Engine
{
    public enum GuiCondition
    {
        Always = 1 << 0,
        Once = 1 << 1,
        FirstUseEver = 1 << 2,
        Appearing = 1 << 3
    }
}
