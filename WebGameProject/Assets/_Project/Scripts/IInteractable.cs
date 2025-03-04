using UnityEngine;

namespace Terminus
{
    public interface IInteractable
    {
        public string InteractMessage { get; }
        public void Interact(RaycastHit obj);
    }
}
