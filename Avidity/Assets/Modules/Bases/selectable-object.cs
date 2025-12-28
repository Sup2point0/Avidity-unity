namespace Avidity
{
    public static partial class Bases
    {
        public enum SelectableObjectType {
            File,
            Track,
            Playlist,
            Artist,
        }

        /// <summary> An object which can be selected in the Selector. </summary>
        public interface ISelectableObject {}
    }
}
