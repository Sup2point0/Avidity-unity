using System.Collections.Generic;
using System.Linq;


public class MultiSet<K> : Dictionary<K, uint>
{
    public MultiSet<K> Add(K value)
    {
        if (base.ContainsKey(value)) {
            this[value]++;
        }
        else {
            base.Add(value, 1);
        }

        return this;
    }

    public List<K> SortedDescending()
        => this
            .OrderByDescending(kvp => kvp.Value)
            .Select(kvp => kvp.Key)
            .ToList();
}
