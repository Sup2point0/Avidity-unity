using UnityEngine;
using UnityEngine.UIElements;


namespace Avidity
{
    public static partial class Bases
    {
        /// <summary> An item inside a <c>ListView</c> which can have a entity bound to it.
        /// </summary>
        /// <typeparam name="Entity">The type of entity the item is bound to.</typeparam>
        public interface IBindableItem<Entity>
        {
            public void InitFromUxml(VisualTreeAsset tree);

            public void Bind(Entity entity);

            public void Unbind();
        }
    }
}
