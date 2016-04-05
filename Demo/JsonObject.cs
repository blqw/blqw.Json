using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class JsonObject : NameValueCollection
    {
        object _Data;
        public JsonObject(object data)
        {
            _Data = data;
        }

        private object Get(object obj, string name, int? index)
        {
            var map = obj as IDictionary;
            if (map != null)
            {
                if (name != null)
                {
                    return Get(map, name);
                }
                return Get(map, index.Value);
            }
            var list = obj as IList;
            if (list != null)
            {
                if (name != null)
                {
                    return Get(list, name);
                }
                return Get(list, index.Value);
            }
            return null;
        }

        private object Get(IList list, int index)
        {
            if (index < list.Count)
            {
                return list[index];
            }
            return null;
        }

        private object Get(IList list, string name)
        {
            int index;
            if (int.TryParse(name, out index))
            {
                return Get(list, index);
            }
            return null;
        }

        private object Get(IDictionary map, int index)
        {
            return map[index.ToString()];
        }

        private object Get(IDictionary map, string name)
        {
            return map[name];
        }

        private IEnumerable<string> ParseNames(string name)
        {
            if (name == null)
            {
                yield return null;
                yield break;
            }
            var chars = name.ToCharArray();
            var start = 0;
            for (int i = 0, length = chars.Length; i < length; i++)
            {
                var c = chars[i];
                switch (c)
                {
                    case '[':
                        yield return new string(chars, start, i - start);
                        i++;
                        start = i;
                        while (i < length)
                        {
                            if (chars[i] == ']')
                            {
                                break;
                            }
                            i++;
                        }
                        if (i == length)
                        {
                            yield return null;
                            yield break;
                        }
                        int index;
                        var str = new string(chars, start, i - start);
                        if (int.TryParse(str, out index))
                        {
                            yield return str;
                        }
                        else
                        {
                            yield return null;
                            yield break;
                        }
                        start = i + 1;
                        break;
                    case '.':
                        yield return new string(chars, start, i - start);
                        start = i + 1;
                        break;
                    default:
                        break;
                }
            }
            if (start != 0 && start != chars.Length)
            {
                yield return new string(chars, start, chars.Length - start);
            }
        }

        public override string Get(string name)
        {
            var obj = _Data;
            foreach (var n in ParseNames(name))
            {
                if (n == null)
                {
                    return Get(_Data, name, null)?.ToString();
                }
                obj = Get(obj, n, null);
                if (obj == null)
                {
                    return Get(_Data, name, null)?.ToString();
                }
            }
            if (ReferenceEquals(obj, _Data))
            {
                return Get(_Data, name, null)?.ToString();
            }
            return obj?.ToString();
        }

        public override string ToString()
        {
            return _Data?.ToString() ?? "";
        }
    }


}
