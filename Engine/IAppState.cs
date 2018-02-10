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
        /// Called the moment this state becomes the active one.
        /// </summary>
        void OnEnter();

        /// <summary>
        /// Called the moment this state leaves the active one.
        /// </summary>
        void OnExit();
    }
}