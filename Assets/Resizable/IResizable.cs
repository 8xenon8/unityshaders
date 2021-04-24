using UnityEditor;
using UnityEngine;

namespace Resizable
{
    public interface IResizable
    {
        void Resize(float sizeDelta);
    }
}