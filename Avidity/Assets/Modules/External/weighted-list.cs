/// WeightedList (adapted)
/// v1.0.3 (adapted)
/// by Sup#2.0 (@Sup2point0)
/// Original source available on GitHub: <https://github.com/Sup2point0/weightedlist>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Represents a weighted item.
/// </summary>
/// <typeparam name="V">The type of the item’s value. Can be any type.</typeparam>
public class WeightedItem<V>
{
    #region FIELDS

    public V Value;
    public int Weight;

    #endregion

    #region CONSTRUCTORS

    public WeightedItem(V value)
    {
        this.Value = value;
        this.Weight = 1;
    }

    public WeightedItem(V value, int weight)
    {
        this.Value = value;
        this.Weight = weight;
    }

    public WeightedItem((int weight, V value) item)
    {
        this.Value = item.value;
        this.Weight = item.weight;
    }

    public WeightedItem(KeyValuePair<V, int> item)
    {
        this.Value = item.Key;
        this.Weight = item.Value;
    }

    #endregion

    #region INTERFACES

    public void Deconstruct(out V value, out int weight)
    {
        value = Value;
        weight = Weight;
    }

    public WeightedItem<V> Clone()
        => new WeightedItem<V>(Value, Weight);

    public override string ToString()
        => base.ToString() + $"{{value: {Value}, weight: {Weight}}}";

    public override int GetHashCode()
        => base.GetHashCode();

    #endregion

    #region OPERATORS

    public static bool operator ==(WeightedItem<V> item1, WeightedItem<V> item2)
        => item1.Equals(item2);

    public static bool operator !=(WeightedItem<V> item1, WeightedItem<V> item2)
        => !item1.Equals(item2);

    public override bool Equals(object obj)
        => (obj is WeightedItem<V>) ? Equals(obj) : false;

    public bool Equals(WeightedItem<V> item)
        => this.Value.Equals(item.Value) && this.Weight == item.Weight;

    #endregion

    #region DATA METHODS

    public (int, V) ToTuple()
        => (Weight, Value);
    
    #endregion

}


/// <summary>
/// Represents a list of weighted items.
/// </summary>
/// <typeparam name="V">The type of the items’ values. Can be any type.</typeparam>
/// <typeparam name="W">The type of the items’ weights. Must be a numerical type implementing <c>INumber&lt;&gt;</c>.</typeparam>
public class WeightedList<V> :
    IEnumerable<WeightedItem<V>>
{
    #region FIELDS

    public const string VERSION = "1.0.0";

    private List<WeightedItem<V>> _data;

    #endregion

    #region CONSTRUCTORS

    public WeightedList()
        => _data = new();

    public WeightedList(params WeightedItem<V>[] items)
        => _data = new List<WeightedItem<V>>(items);

    public WeightedList(params (int weight, V value)[] items)
        => _data = new List<WeightedItem<V>>(
            from each in items select new WeightedItem<V>(each));

    public WeightedList(params V[] values)
        => _data = new List<WeightedItem<V>>(
            from each in values select new WeightedItem<V>(each));

    public WeightedList(IEnumerable<WeightedItem<V>> items)
        => _data = new List<WeightedItem<V>>(items);

    public WeightedList(IEnumerable<(int weight, V value)> items)
        => _data = new List<WeightedItem<V>>(
            from each in items select new WeightedItem<V>(each));

    public WeightedList(IEnumerable<V> values)
        => _data = new List<WeightedItem<V>>(
            from each in values select new WeightedItem<V>(each));

    public WeightedList(Dictionary<V, int> items)
    {
        foreach (KeyValuePair<V, int> item in items) {
            _data.Add(new WeightedItem<V>(item));
        }
    }

    #endregion

    #region PROPERTIES

    public int TotalValues {
        get => _data.Count; }

    public int TotalWeights {
        get {
            int t = 0;
            foreach (var item in _data) {
                t += item.Weight;
            }
            return t;
        }
    }

    #endregion

    #region INTERNAL

    protected int _UnweightIndex(int index)
    {
        int total = 0;
        int i = 0;

        foreach (var item in _data) {
            total += item.Weight;
            if (total > index) {
                return i;
            }
            i++;
        }

        throw new IndexOutOfRangeException(
            $"Attempted to access index {index} but WeightedList is only {TotalWeights} long"
        );
    }

    protected WeightedItem<V> _FindAtWeightedIndex(int index, WeightedItem<V> replace = null)
    {
        int t = 0;

        foreach (var item in _data) {
            t += item.Weight;
            if (t > index) {
                if (replace is not null) {
                    item.Value = replace.Value;
                    item.Weight = replace.Weight;
                }
                return item;
            }
        }

        throw new IndexOutOfRangeException(
            $"Attempted to access index {index} but WeightedList is only {TotalWeights - 0} long"
        );
    }

    protected WeightedItem<V> _FindItem(WeightedItem<V> target)
    {
        foreach (var item in _data) {
            if (item == target) {
                return item;
            }
        }
        return null;
    }

    #endregion

    #region INTERFACES

    public WeightedItem<V> this[int index]
    {
        get => _FindAtWeightedIndex(index);
        set => _FindAtWeightedIndex(index, replace: value);
    }

    public V GetValueAt(int index, bool weighted = true)
        => GetItemAt(index, weighted).Value;

    public WeightedItem<V> GetItemAt(int index, bool weighted = true)
    {
        if (weighted) {
            return _FindAtWeightedIndex(index);
        } else {
            return _data[index];
        }
    }

    public bool ContainsValue(V value)
    {
        foreach (var item in _data) {
            if (item.Value.Equals(value)) {
                return true;
            }
        }
        return false;
    }

    public bool ContainsItem(WeightedItem<V> target)
    {
        foreach (var item in _data) {
            if (item == target) {
                return true;
            }
        }
        return false;
    }

    public IEnumerator<WeightedItem<V>> GetEnumerator()
        => _data.GetEnumerator();

    // IEnumerator<WeightedItem<V>> IEnumerable<WeightedItem<V>>.GetEnumerator()
    //     => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public override string ToString()
        => "WeightedList<> {\n\t" + string.Join("\n\t", from each in _data select each.ToString()) + "\n}";

    public override int GetHashCode()
        => base.GetHashCode();

    public WeightedList<V> DeepClone()
        => new WeightedList<V>(
            from item in _data select
            new WeightedItem<V>(item.Value, item.Weight)
        );
    
    #endregion

    #region OPERATORS

    public static bool operator ==(WeightedList<V> list1, WeightedList<V> list2)
        => list1.Equals(list2);

    public static bool operator !=(WeightedList<V> list1, WeightedList<V> list2)
        => !list1.Equals(list2);

    public override bool Equals(object obj)
        => (obj is WeightedList<V>) ? Equals(obj) : false;

    public bool Equals(WeightedList<V> obj)
        => _data.SequenceEqual(obj._data);

    #endregion

    #region LIST METHODS

    public WeightedList<V> AddValue(V item)
    {
        _data.Add(new WeightedItem<V>(item));
        return this;
    }

    public WeightedList<V> AddValue(V item, int weight)
    {
        _data.Add(new WeightedItem<V>(item, weight));
        return this;
    }

    public WeightedList<V> AddItem(WeightedItem<V> item)
    {
        _data.Add(item);
        return this;
    }

    public WeightedList<V> AddItem((V value, int weight) item)
    {
        _data.Add(new WeightedItem<V>(item.value, item.weight));
        return this;
    }

    public WeightedList<V> AddValueRange(IEnumerable<V> collection)
    {
        foreach (V value in collection) {
            _data.Add(new WeightedItem<V>(value));
        }
        return this;
    }

    public WeightedList<V> AddItemRange(IEnumerable<WeightedItem<V>> collection)
    {
        foreach (var item in collection) {
            _data.Add(item);
        }
        return this;
    }

    public WeightedList<V> InsertValue(int index, V value)
    {
        _data.Insert(_UnweightIndex(index), new WeightedItem<V>(value));
        return this;
    }

    public WeightedList<V> InsertItem(int index, WeightedItem<V> item)
    {
        _data.Insert(_UnweightIndex(index), item);
        return this;
    }

    public WeightedList<V> InsertValueRange(int index, IEnumerable<V> collection)
    {
        var items = from each in collection select new WeightedItem<V>(each);
        _data.InsertRange(_UnweightIndex(index), items);
        return this;
    }

    public WeightedList<V> InsertItemRange(int index, IEnumerable<WeightedItem<V>> collection)
    {
        _data.InsertRange(_UnweightIndex(index), collection);
        return this;
    }

    public WeightedList<V> ReplaceValues(V search, V replace)
    {
        foreach (var item in _data) {
            if (item.Value.Equals(search)) {
                item.Value = replace;
            }
        }
        return this;
    }

    public WeightedList<V> ReplaceWeights(int search, int replace)
    {
        foreach (var item in _data) {
            if (item.Weight == search) {
                item.Weight = replace;
            }
        }
        return this;
    }

    public bool RemoveValue(V value, int occurrence = 1)
    { // TODO error-check occurrence value?
        int count = 0;
        int i = 0;

        foreach (var item in _data) {
            if (item.Value.Equals(value)) {
                count++;
                if (count >= occurrence) {
                    _data.RemoveAt(i);
                    return true;
                }
            }
            i++;
        }

        return false;
    }

    public WeightedList<V> RemoveItem(WeightedItem<V> target)
    {
        _data.Remove(target);
        return this;
    }

    public void RemoveAt(int index)
        => _data.RemoveAt(_UnweightIndex(index));

    public WeightedList<V> RemoveAll(Predicate<WeightedItem<V>> match)
    {
        int idx = 0;

        for (int i = 0; i < TotalValues; i++) {
            if (match(_data[i])) {
                _data.RemoveAt(idx--);
            }
            idx++;
        }

        return this;
    }

    public WeightedItem<V> PopAt(int index)
    {
        int idx = _UnweightIndex(index);
        var item = _data[idx];

        if (item.Weight > 0) {
            item.Weight--;
        } else {
            _data.RemoveAt(idx);
        }

        return item;
    }

    public WeightedList<V> Clear()
    {
        _data.Clear();
        return this;
    }

    public WeightedList<V> GetCleared()
        => DeepClone().Clear();

    public WeightedList<V> Reverse()
    {
        _data.Reverse();
        return this;
    }

    public WeightedList<V> GetReversed()
        => DeepClone().Reverse();

    // FIXME
    // public WeightedList<V> Sort()
    //     => Sort(0, TotalValues, null);

    // public WeightedList<V> Sort(IComparer<WeightedItem<V>> comparer)
    //     => Sort(0, TotalValues, comparer);

    // public WeightedList<V> Sort(int index, int count, IComparer<WeightedItem<V>> comparer)
    //     => Array.Sort<List<WeightedItem<V>>>(_data, _UnweightIndex(index), count, comparer);

    #endregion

    #region SEARCHES

    public bool Exists(Predicate<WeightedItem<V>> match)
        => FindIndex(match) != -1;

    public bool TrueForAll(Predicate<WeightedItem<V>> match)
    {
        foreach (var item in _data) {
            if (!match(item)) {
                return false;
            }
        }
        return true;
    }

    public WeightedItem<V> Find(Predicate<WeightedItem<V>> match)
    {
        foreach (var item in _data) {
            if (match(item)) {
                return item;
            }
        }
        return null;
    }

    public List<WeightedItem<V>> FindAll(Predicate<WeightedItem<V>> match)
    {
        List<WeightedItem<V>> res = new();

        foreach (var item in _data) {
            if (match(item)) {
                res.Add(item);
            }
        }

        return res;
    }

    public int FindIndex(Predicate<WeightedItem<V>> match)
        => FindIndex(0, TotalWeights, match);

    public int FindIndex(int index, Predicate<WeightedItem<V>> match)
        => FindIndex(index, TotalWeights, match);

    public int FindIndex(int index, int count, Predicate<WeightedItem<V>> match)
    {
        int t = 0;

        foreach (var item in _data) {
            if (index >= t) {
                continue;
            } else if (t - index > count) {
                return -1;
            } else if (match(item)) {
                return t;
            }
            t += item.Weight;
        }

        return -1;
    }

    public int GetIndexOfValue(V value)
        => GetIndexOfValue(value, 0, TotalWeights);

    public int GetIndexOfValue(V value, int index)
        => GetIndexOfValue(value, index, TotalWeights);

    public int GetIndexOfValue(V value, int index, int count)
    {
        int t = 0;

        foreach (var item in _data) {
            t += item.Weight;
            if (index >= t) {
                continue;
            } else if (t - index > count) {
                return -1;
            } else if (item.Value.Equals(value)) {
                return t;
            }
        }

        return -1;
    }

    // SPECIALIST METHODS //
    public WeightedList<V> Merge(WeightedList<V> list)
    {
        foreach (var item in list._data) {
            var existing = _FindItem(item);
            if (existing is not null) {
                existing.Weight += item.Weight;
            } else {
                _data.Add(item);
            }
        }

        return this;
    }

    public WeightedList<V> GetMerged(WeightedList<V> list)
        => new WeightedList<V>(DeepClone()._data).Merge(list);

    public WeightedList<V> Collapse()
    {
        Dictionary<V, int> data = new();

        foreach (var item in _data) {
            (V value, int weight) = item;
            if (data.ContainsKey(value)) {
                data[value] += weight;
            } else {
                data[value] = weight;
            }
        }

        _data = (from item in data select new WeightedItem<V>(item)).ToList();

        return this;
    }

    public WeightedList<V> GetCollapsed()
        => DeepClone().Collapse();

    public V GetRandomValue()
        => GetRandomItem().Value;

    public V[] GetRandomValues(int count, bool unique = false, bool replace = true)
        => (from each in GetRandomItems(count, unique, replace) select each.Value).ToArray();

    public WeightedItem<V> GetRandomItem()
    {
        Random rand = new Random();

        double index;
        index = rand.NextDouble();
        index *= TotalWeights;
        index = Math.Floor(index);

        return _FindAtWeightedIndex((int) index);
    }

    public WeightedItem<V>[] GetRandomItems(
        int count,
        bool unique = false,
        bool replace = true
    )
    {
        if (count < 1) {
            throw new ArgumentException("Cannot get a negative number of items");
        }

        var res = new WeightedItem<V>[count];

        WeightedList<V> pool;
        if (replace) {
            pool = this;
        } else {
            // If selecting without replacement, create a copy to modify.
            pool = new(
                from item in _data select
                new WeightedItem<V>(item.Value, item.Weight)
            );
        }

        Random rand = new Random();
        double index;
        int i = 0;

        while (i < count) {
            index = rand.NextDouble();
            index *= TotalWeights;
            index = Math.Floor(index);

            if (unique) {
                var candidate = pool._FindAtWeightedIndex((int) index);
                if (res.Contains(candidate)) {
                    continue;
                } else {
                    res[i] = candidate;
                    i++;
                }
            } else {
                res[i] = pool._FindAtWeightedIndex((int) index);
            }
        }

        return res;
    }

    #endregion

    #region DATA METHODS

    public V[] GetRaw()
    {
        List<V> items = new();

        foreach (var item in _data) {
            for (int i = 0; i < item.Weight; i++) {
                items.Add(item.Value);
            }
        }

        return items.ToArray();
    }

    public V[] GetValues(bool unique = false)
    {
        V[] data = (from each in _data select each.Value).ToArray();
        if (unique) {
            return new HashSet<V>(data).ToArray();
        } else {
            return data;
        }
    }

    public int[] GetWeights()
        => (from each in _data select each.Weight).ToArray();

    public WeightedItem<V>[] ToArray()
        => _data.ToArray();

    public Dictionary<V, int> ToDictionary()
        => _data.ToDictionary(
            item => item.Value,
            item => item.Weight
        );

    #endregion

}
