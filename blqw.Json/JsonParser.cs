using System;
using System.Collections.Generic;
using System.Collections;

namespace blqw
{
    /// <summary> 用于将Json字符串转换为C#对象
    /// </summary>
    public sealed class JsonParser
    {
        /// <summary> 将json字符串转换为指定对象
        /// </summary>
        public Object ToObject(Type type, string jsonString)
        {
            if (jsonString == null || jsonString.Length == 0)
            {
                return null;
            }
            if (type == null)
            {
                object obj = null;
                FillObject(ref obj, typeof(Dictionary<string, object>), jsonString);
                return obj;
            }
            else if (type.IsArray)
            {
                object arr = new ArrayList();
                FillObject(ref arr, type, jsonString);
                return ((ArrayList)arr).ToArray(type.GetElementType());
            }
            else
            {
                object obj = null;
                FillObject(ref obj, type, jsonString);
                return obj;
            }
        }

        /// <summary> 将json字符串中的数据填充到指定对象,obj为null抛出异常
        /// </summary>
        public void FillObject(object obj, string jsonString)
        {
            Assertor.AreNull(obj, "obj");
            if (jsonString != null && jsonString.Length > 0)
            {
                FillObject(ref obj, obj.GetType(), jsonString);
            }
        }


        /* 以下方法私有 */

        /// <summary> 将json字符串中的数据填充到指定对象
        /// </summary>
        /// <param name="obj">将数据填充到对象,如果对象为null且json不是空字符串则创建对象并返回</param>
        /// <param name="type">obj的对象类型,该参数不能为null,且必须obj的类型或父类或接口</param>
        /// <param name="jsonString">json字符串</param>
        private void FillObject(ref object obj, Type type, string jsonString)
        {
            unsafe
            {
                fixed (char* p = jsonString)
                {
                    using (UnsafeJsonReader reader = new UnsafeJsonReader(p, jsonString.Length))
                    {
                        //如果是由空白和回车组成的字符串,直接返回
                        if (reader.IsEnd()) return;
                        FillObject(ref obj, type, reader);
                        if (reader.IsEnd() == false)
                        {
                            ThrowException("json字符串正确结束");
                        }
                    }
                }
            }
        }

        /// <summary>  解释 json 字符串,并填充到obj对象中,如果obj为null则新建对象
        /// </summary>
        private void FillObject(ref object obj, Type type, UnsafeJsonReader reader)
        {
            reader.CheckEnd();

            var jsonType = JsonType.Get(type);

            //如果obj == null创建新对象
            if (obj == null)
            {
                if (jsonType.TypeCodeEx == TypeCodeEx.AnonymousType)
                {
                    throw new NotSupportedException("不支持匿名类型的反序列化操作");
                }
                obj = jsonType.CreateInstance();
            }

            //判断起始字符
            switch (reader.Current)
            {
                case '{':
                    reader.MoveNext();
                    if (jsonType.IsDictionary)
                    {
                        FillDictionary(obj, jsonType, reader);
                    }
                    else
                    {
                        FillProperty(obj, jsonType, reader);
                    }
                    reader.SkipChar('}', true);
                    break;
                case '[': // 中括号的json仅支持反序列化成IList的对象
                    reader.MoveNext();
                    if (jsonType.Type.IsArray)
                    {
                        if (obj is ArrayList)
                        {
                            FillList(obj, jsonType, reader);
                        }
                        else
                        {
                            var arr = obj as Array;
                            if (arr == null)
                            {
                                ThrowException("无法处理当前对象 : 类型( " + ExtendMethod.DisplayName(obj.GetType()) + " )");
                            }
                            FillArray(arr, jsonType, reader);
                        }
                    }
                    else if (jsonType.IsList)
                    {
                        FillList(obj, jsonType, reader);
                    }
                    else
                    {
                        ThrowException("无法处理当前对象 : 类型( " + ExtendMethod.DisplayName(obj.GetType()) + " )");
                    }
                    reader.SkipChar(']', true);
                    break;
                default:
                    ThrowException("Json字符串必须以 { 或 [ 开始");
                    break;
            }

        }

        /// <summary> 填充一般对象的属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="jsonType"></param>
        /// <param name="reader"></param>
        private void FillProperty(object obj, JsonType jsonType, UnsafeJsonReader reader)
        {
            if (reader.SkipChar('}', false))
            {
                return;
            }
            do
            {
                var key = ReadKey(reader);                      //获取Key
                var member = jsonType[key, true];               //得到对象属性
                if (member != null && member.CanWrite)
                {
                    object val = ReadValue(reader, member.Type);//得到值
                    member.Member.SetValue(obj, val);           //赋值
                }
                else
                {
                    SkipValue(reader);                          //跳过Json中的值
                }
            } while (reader.SkipChar(',', false));
        }

        private void FillArray(Array arr, JsonType jsonType, UnsafeJsonReader reader)
        {
            if (reader.SkipChar(']', false) || arr.Length == 0)
            {
                return;
            }

            var length = arr.Length;
            var eleType = jsonType.ElementType;
            for (int i = 0; i < length; i++)
            {
                object val = ReadValue(reader, eleType);  //得到值
                arr.SetValue(val, i);
                if (reader.SkipChar(',', false) == false)
                {
                    return;
                }
            }
        }

        /// <summary> 填充 IDictionary 或者 IDictionary&lt;,&gt;
        /// </summary>
        /// <param name="obj">IDictionar或IDictionary<,>实例</param>
        /// <param name="jsonType"></param>
        /// <param name="reader"></param>
        private void FillDictionary(object obj, JsonType jsonType, UnsafeJsonReader reader)
        {
            if (reader.SkipChar('}', false))
            {
                return;
            }
            if (jsonType.AddKeyValue == null)
            {
                ThrowException("对象无法写入数据,对象有可能是只读的或找不到Add入口");
            }
            var eleType = jsonType.ElementType;
            var keyType = jsonType.KeyType;
            if (keyType == typeof(string) || keyType == typeof(object))
            {
                do
                {
                    string key = ReadKey(reader);               //获取Key
                    object val = ReadValue(reader, eleType);    //得到值
                    jsonType.AddKeyValue(obj, key, val);
                } while (reader.SkipChar(',', false));
            }
            else
            {
                var conv = StringConverter.CreateConverter(keyType, true);
                do
                {
                    string keyStr = ReadKey(reader);            //获取Key
                    object key = conv(keyStr);
                    object val = ReadValue(reader, eleType);    //得到值
                    jsonType.AddKeyValue(key, val);
                } while (reader.SkipChar(',', false));
            }
        }

        /// <summary> 填充 IList 或者 IList &lt;&gt;
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="jsonType"></param>
        /// <param name="reader"></param>
        private void FillList(object obj, JsonType jsonType, UnsafeJsonReader reader)
        {
            if (reader.SkipChar(']', false))
            {
                return;
            }
            if (jsonType.AddValue == null)
            {
                ThrowException("对象无法写入数据,对象有可能是只读的或找不到Add入口");
            }
            var eleType = jsonType.ElementType;
            do
            {
                object val = ReadValue(reader, eleType);  //得到值
                jsonType.AddValue(obj, val);                  //赋值
            } while (reader.SkipChar(',', false));
        }

        /// <summary> 跳过一个键
        /// </summary>
        /// <param name="reader"></param>
        private static void SkipKey(UnsafeJsonReader reader)
        {
            reader.CheckEnd();
            if (reader.Current == '"' || reader.Current == '\'')
            {
                reader.SkipString();
            }
            else
            {
                reader.SkipWord();
            }
            reader.SkipChar(':', true);
        }

        /// <summary> 获取一个键
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static string ReadKey(UnsafeJsonReader reader)
        {
            reader.CheckEnd();
            string key;
            if (reader.Current == '"' || reader.Current == '\'')
            {
                key = reader.ReadString();
            }
            else
            {
                key = reader.ReadWord();
            }
            reader.SkipChar(':', true);
            return key;
        }



        /// <summary> 跳过一个值
        /// </summary>
        /// <param name="reader"></param>
        private static void SkipValue(UnsafeJsonReader reader)
        {
            reader.CheckEnd();
            switch (reader.Current)
            {
                case '[':
                    reader.MoveNext();
                    if (reader.SkipChar(']', false))
                    {
                        return;
                    }
                    do
                    {
                        SkipValue(reader);
                    } while (reader.SkipChar(',', false));
                    reader.SkipChar(']', true);
                    break;
                case '{':
                    reader.MoveNext();
                    if (reader.SkipChar('}', false))
                    {
                        return;
                    }
                    do
                    {
                        SkipKey(reader);
                        SkipValue(reader);
                    } while (reader.SkipChar(',', false));
                    reader.SkipChar('}', true);
                    break;
                case '"':
                case '\'':
                    reader.SkipString();
                    break;
                default:
                    reader.ReadConsts();
                    break;
            }
        }

        /// <summary> 读取一个值对象
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private object ReadValue(UnsafeJsonReader reader, Type type)
        {
            reader.CheckEnd();
            switch (reader.Current)
            {
                case '[':
                    reader.MoveNext();
                    var array = ReadList(reader, type);
                    reader.SkipChar(']', true);
                    return array;
                case '{':
                    reader.MoveNext();
                    var obj = ReadObject(reader, type);
                    reader.SkipChar('}', true);
                    return obj;
                case '"':
                case '\'':
                    if (type == typeof(DateTime))
                    {
                        return reader.ReadDateTime();
                    }
                    return ParseString(reader, type);
                default:
                    object val = reader.ReadConsts();
                    return DataConverter.ChangedType(val, type, true);
            }
        }

        /// <summary> 将字符串解析为指定类型
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object ParseString(UnsafeJsonReader reader, Type type)
        {
            var typecode = Type.GetTypeCode(type);
            //数字
            if ((typecode >= TypeCode.SByte && typecode <= TypeCode.Decimal) || typecode == TypeCode.Boolean)
            {
                //枚举
                if (type.IsEnum)
                {
                    return Enum.Parse(type, reader.ReadString());
                }
                char quot = reader.Current;
                reader.MoveNext();
                var val = DataConverter.ChangedType(reader.ReadConsts(), type, true);
                reader.SkipChar(quot, true);
                return val;
            }
            else if (typecode == TypeCode.DateTime)
            {
                return reader.ReadDateTime();
            }
            return StringConverter.ChangedType(reader.ReadString(), type, true);
        }


        private static JsonType JsonTypeArrayList = JsonType.Get(typeof(ArrayList));
        /// <summary> 读取数组
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private object ReadList(UnsafeJsonReader reader, Type type)
        {
            var jsonType = JsonType.Get(type);
            if (type.IsArray)
            {
                ArrayList list = new ArrayList();   //Array类型中的AddValue方法调用的是ArrayList
                FillList(list, jsonType, reader);
                return list.ToArray(jsonType.ElementType);
            }
            if (type == typeof(object))
            {
                ArrayList list = new ArrayList();
                FillList(list, JsonTypeArrayList, reader);
                return list;
            }
            else
            {
                var list = jsonType.CreateInstance();
                FillList(list, jsonType, reader);
                return list;
            }
        }

        private static JsonType JsonTypeDictionary = JsonType.Get(typeof(Dictionary<string, object>));
        /// <summary> 读取对象
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private object ReadObject(UnsafeJsonReader reader, Type type)
        {
            var jsonType = JsonType.Get(type);
            if (type == typeof(object))
            {
                var obj = new Dictionary<string, object>();
                FillDictionary(obj, JsonTypeDictionary, reader);
                return obj;
            }
            else if (jsonType.IsDictionary)
            {
                var obj = jsonType.CreateInstance();
                FillDictionary(obj, jsonType, reader);
                return obj;
            }
            else
            {
                var obj = jsonType.CreateInstance();
                FillProperty(obj, jsonType, reader);
                return obj;
            }
        }


        private static void ThrowException(string word)
        {
            throw new NotSupportedException("无法解析:" + word);
        }

    }
}
