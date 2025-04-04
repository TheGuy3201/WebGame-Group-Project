using System;

namespace Terminus
{
    public static class GameEvents
    {
        public static event Action<string> OnZoneEntered;

        public static void ZoneEntered(string zoneName)
        {
            OnZoneEntered?.Invoke(zoneName);
        }
    }

}
