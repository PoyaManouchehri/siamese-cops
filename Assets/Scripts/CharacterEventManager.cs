using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class CharacterEventManager : MonoBehaviour
    {
        public event EventHandler Shot = delegate { };
        public event EventHandler Tazed = delegate { };

        public void RaiseShot()
        {
            Shot(this, new EventArgs());
        }

        public void RaiseTazed()
        {
            Tazed(this, new EventArgs());
        }
    }
}
