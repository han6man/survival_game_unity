using UnityEngine;

namespace SpaceInvaders.Logic
{
    public class MapGrid
    {
        private Vector2[] _vecPositions = new Vector2[]
        {
           Vector2.zero,
           new Vector2 (0, 1),
           new Vector2 (1, 0),
           new Vector2 (1, 1),
        };

        public Vector2[] VectorPositions
        {
            get { return _vecPositions; }
        }

        private Vector2? _blockedPose = null;
        public Vector2? BlockedPose
        {
            get { return _blockedPose; }
            set { _blockedPose = value; }
        }

        public MapGrid()
        {
            
        }

        public MapGrid(int size)
        {
            
        }

        public MapGrid(int size, Vector2 blockedLocation) : this(size)
        {
            BlockedPose = blockedLocation;
        }

    }
}
