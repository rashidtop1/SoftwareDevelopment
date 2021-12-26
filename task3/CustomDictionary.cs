using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareDevelopment
{
    class CustomDictionary<Tkey, Tvalue> : IDictionary<Tkey, Tvalue>
    {
         public struct Entry
         {
            public Tvalue value;
            public int _hash;
            public Tkey key;
            
         }

        private LinkedList<Entry>[] Hash;

        public CustomDictionary()
        {
            Hash = new LinkedList<Entry>[10];
            for (int i = 0; i < 10; ++i)
            {
                Hash[i] = new LinkedList<Entry>();
            }
        }

        
        public ICollection<Tkey> Keys
        {
            get
            {
                List<Tkey> keys = new List<Tkey>();
                foreach (var list in Hash)
                {
                    foreach (var elements in list)
                    {
                        keys.Add(elements.key);
                    }
                }
                return keys;
            }
        }

        public Tvalue this[Tkey key]
        {
            get
            {
                int _hash = key.GetHashCode();
                int index = ((_hash % Hash.Length) + Hash.Length) % Hash.Length;
                foreach (Entry i in Hash[index])
                {
                    if (i.key.Equals(key))
                    {
                        return i.value;
                    }
                }
                throw new Exception($"Ключ {key} не найден.");
            }
            set
            {
                int _Hash = key.GetHashCode();
                int index = ((_Hash % Hash.Length) + Hash.Length) % Hash.Length;
                Entry temp = new Entry
                {
                    _hash = _Hash,
                    key = key,
                    value = value
                };

                for (var variable = Hash[index].First; !variable.Equals(Hash[index].Last); variable = variable.Next)
                {
                    if (variable.Value.key.Equals(key))
                    {
                        variable.Value = temp;
                        return;
                    }
                }
                Add(key, value);
            }
        }


        public ICollection<Tvalue> Values
        {
            get
            {
                List<Tvalue> values = new List<Tvalue>();
                foreach (var list in Hash)
                {
                    foreach (var elements in list)
                    {
                        values.Add(elements.value);
                    }
                }
                return values;
            }
        }

        public int Count => Hash.Where(x => x != null).Select(x => x.Count).Sum();

        public bool IsReadOnly => false;

        public void Add(Tkey key, Tvalue value)
        {
            int HASH = key.GetHashCode();
            int index = ((HASH % Hash.Length) + Hash.Length) % Hash.Length;
            if (Hash[index].Any(x => x.key.Equals(key)))
            {
                throw new ArgumentException("REPEAT VALUE");
            }
            Hash[index].AddLast(new Entry { _hash = HASH, key = key, value = value });
            int CNT_VALUE = Hash.Count(x => x.Count == 0);
            if (Hash[index].Count >= 5 || CNT_VALUE < Hash.Length / 3)
            {
                LinkedList<Entry>[] _HASHTABLE = new LinkedList<Entry>[Hash.Length * 2];
                for (int i = 0; i < _HASHTABLE.Length; ++i)
                {
                    _HASHTABLE[i] = new LinkedList<Entry>();
                }
                foreach (var list in Hash)
                {
                    foreach (var elem in list)
                    {
                        HASH = elem._hash;
                        index = ((HASH % _HASHTABLE.Length) + _HASHTABLE.Length) % _HASHTABLE.Length;
                        _HASHTABLE[index].AddLast(elem);
                    }
                }
                Hash = _HASHTABLE;
            }

        }

        public void Add(KeyValuePair<Tkey, Tvalue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            Hash = new LinkedList<Entry>[10];
        }

        public bool Contains(KeyValuePair<Tkey, Tvalue> item)
        {
            int _hash = item.Key.GetHashCode();
            int index = ((_hash % Hash.Length) + Hash.Length) % Hash.Length;
            Entry curr = new Entry { _hash = _hash, key = item.Key, value = item.Value };
            return Hash[index].Contains(curr);
        }

        public bool ContainsKey(Tkey key)
        {
            int _hash = key.GetHashCode();
            int index = ((_hash % Hash.Length) + Hash.Length) % Hash.Length;
            return Hash[index].Any(x => x.key.Equals(key));
        }



        public IEnumerator<KeyValuePair<Tkey, Tvalue>> GetEnumerator()
        {
            foreach (var list in Hash)
            {
                foreach (var elem in list)
                {
                    yield return new KeyValuePair<Tkey, Tvalue>(elem.key, elem.value);
                }
            }
        }


        public void CopyTo(KeyValuePair<Tkey, Tvalue>[] arr, int arrIndex)
        {
            foreach (var list in Hash)
            {
                foreach (var elem in list)
                {
                    arr[arrIndex] = new KeyValuePair<Tkey, Tvalue>(elem.key, elem.value);
                    arrIndex++;
                }
            }
        }


        public bool Remove(KeyValuePair<Tkey, Tvalue> item)
        {
            int _hash = item.Key.GetHashCode();
            int index = ((_hash % Hash.Length) + Hash.Length) % Hash.Length;
            Entry currEntry = new Entry { _hash = _hash, key = item.Key, value = item.Value };
            if (Hash[index].Contains(currEntry))
            {
                return Hash[index].Remove(currEntry);
            }
            return false;
        }


        public bool Remove(Tkey key)
        {
            int _hash = key.GetHashCode();
            int index = ((_hash % Hash.Length) + Hash.Length) % Hash.Length;
            if (Hash[index].Any(x => x.key.Equals(key)))
            {
                Entry currentEntry = new Entry();
                foreach (Entry i in Hash[index])
                {
                    if (i.key.Equals(key))
                    {
                        currentEntry = i;
                        break;
                    }
                }
                return Hash[index].Remove(currentEntry);
            }
            return false;
        }

        
                
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<Tkey, Tvalue>>)this).GetEnumerator();
        }

        public bool TryGetValue(Tkey key, [MaybeNullWhen(false)] out Tvalue value)
        {
            int _hash = key.GetHashCode();
            int index = ((_hash % Hash.Length) + Hash.Length) % Hash.Length;
            foreach (var elements in Hash[index])
            {
                if (elements.key.Equals(key))
                {
                    value = elements.value;
                    return true;
                }
            }
            value = default(Tvalue);
            return false;
        }
    }
}
