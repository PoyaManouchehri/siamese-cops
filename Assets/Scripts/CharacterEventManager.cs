using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class CharacterEventManager : MonoBehaviour
    {
        public event EventHandler Shot = delegate { };
        public event EventHandler FatallyShot = delegate { };
        public event EventHandler Tazed = delegate { };
        public event EventHandler<PickedUpHealthEventArgs> PickedUpHealth = delegate { };
        public event EventHandler Revived = delegate { };

        public void RaiseShot()
        {
            Shot(this, new EventArgs());
        }

        public void RaiseFatallyShot()
        {
            FatallyShot(this, new EventArgs());
        }

        public void RaiseTazed()
        {
            Tazed(this, new EventArgs());
        }

        public void RaisePickedUpHealth(float speedMultiplier)
        {
            PickedUpHealth(this, new PickedUpHealthEventArgs(speedMultiplier));
        }

        public void RaiseRevived()
        {
            Revived(this, new EventArgs());
        }
    }

    public class PickedUpHealthEventArgs : EventArgs
    {
        public float SpeedMultiplier { get; private set; }

        public PickedUpHealthEventArgs(float speedMultiplier)
        {
            SpeedMultiplier = speedMultiplier;
        }
    }
}
