using System.Drawing;

namespace winform2334123123
{
    public class TileTypeBase
    {
        public enum ClickAction
        {
            None,
            ChangeType,
            Explode
        }

        public Color colour;
        public ClickAction clickAction;
        public TileTypeBase subType;
        public TileTypeBase flagged;
    }
}
