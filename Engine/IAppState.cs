using System;
using System.Numerics;

namespace Engine
{
    public interface IAppState
    {
        /// <summary>
        /// Called at the start of each frame.
        /// </summary>
        void OnFrame();

        /// <summary>
        /// Called when this state begins.
        /// </summary>
        void OnBegin();

        /// <summary>
        /// Called when quiting this state.
        /// </summary>
        void OnEnd();
    }
}