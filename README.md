# blqw.Json 一个优秀的Json序列化类库
Json序列化与反序列化  
http://cnblogs.com/blqw/p/json.html  
使用方便 ,性能卓越  
```csharp
blqw.Json.ToJsonString(object);  
blqw.Json.ToObject<T>(string);  
blqw.Json.ToObject(string);  
blqw.Json.ToObject(Type,string);  
//2014.07.29 新增  
blqw.Json.ToJsonObject(string);  
//2014.08.08 新增  
blqw.Json.ToJsonString(object,JsonBuilderSettings);  
//2014.10.25 新增  
blqw.Json.ToDynamic(string);  
```

性能测试请参考:[pk.md](https://github.com/blqw/blqw.Json/blob/master/pk.md)

##更新说明
#### 2016.02.21  
* 优化IoC模块  

#### 2015.11.06
* 改为可独立运行,辅助功能依靠IoC插件方式加载

#### 2015.06.21  
* 重新设计 Dynamic 类型的实现方式,现在可以将属性区分大小写,可以在运行时正确推断类型  
* 取消 `IJsonObject` 接口  
* 取消 `ILoadJson` 接口  
  
#### 2015.06.13  
* 增加序列化时可以选择输出类型信息的选项,反序列化的时候可以优先尝试序列化为指定的对象  

#### 2015.06.10  
*  优化类型转换部分,全面采用convert3组件方案,修改部分代码
*  小幅度优化字符串拼接部分的性能

#### 2014.12.21
*  优化IgnoreNullMember的处理方式,现在可以正确的忽略Dictionary或者DataTable中空的属性 

#### 2014.12.20
* 优化`IList`的序列化性能  
* 优化`IJsonObject`,现在实现它也必须现实`IEnumerable<IJsonObject>`接口,所以可以直接`foreach`操作,例如下面这段代码  
```csharp
    public void Fill(blqw.IJsonObject jsonObject)
    {
        var items = jsonObject["Items"];
        if (items != null)
        {
            foreach (var item in items)
            {
                Add(item.ToString());
            }
        }
        var total = jsonObject["TotalCount"] as IConvertible;
        if (total != null)
        {
            TotalCount = total.ToInt32(null);
        }
    } 
```  
* 新增2个接口`IToJson`,`ILoadJson`允许对象自定义序列化和反序列化的部分行为  
* 同步更新`Literacy`,增加2个转换全半角的方法  

#### 2014.12.03
* 修正`void AppendDataReader(IDataReader reader)`方法命名错误的问题(一直写成`void AppendDataSet(IDataReader reader)`)
* 修正 序列化IDataReader 时的数据,为`[{"columnName1":"value1","columnName2":"value2"},{"columnName1":"value3","columnName2":"value4"}]`(之前是`{"columns":["columnName1","columnName2"],"values":[["value1","value2"],["value3","value4"]]}`)

#### 2014.11.28
* 更新一处bug在序列化-10的时候会出现错误 感谢网友@孩子，抬起头  

#### 2014.10.27
* 完美解决了,引用项目的问题

#### 2014.10.25
* 感谢 @冲动 让我有了灵感,现在可以支持dynamic了
* 因为blqw.Json项目是2.0开发的,如果4.0项目引用可以使用dynamic,否则会抛出"dynamic类型加载失败!"的异常

```csharp
	var str = "{ Name : \"blqw\", Age : 11 ,Array : [\"2014-1-1 1:00:00\",false,{ a:1,b:2 }] }";
	dynamic json = Json.ToDynamic(str);
	Console.WriteLine(json.Name);
	Console.WriteLine(json.Age);
	Console.WriteLine(((DateTime)json.Array[0]).ToShortDateString());
	Console.WriteLine(((bool)json.Array[1]) == false);
	Console.WriteLine(json.Array[2].a);
	Console.WriteLine(json.Array[2].b);
```

#### 2014.09.14
* 同步更新 [blqw.Literacy]

#### 2014.08.31  
* 优化类型转换,性能up!

#### 2.0b1 版本发布
  
#### 2014.09.10  
* 更新Literacy组件中一处兼容问题,在某些版本的windows下会造成初始化失败的问题

#### 2014.09.09  
* 同步更新Literacy组件  
* 更新两处序列化错误的问题  

#### 2014.08.08   
* 优化代码 :  
    1. 放弃直接使用JsonBuilder ,现在JsonBuilder仅作为抽象基类存在  
    1. 放弃使用Dictionary检测循环引用,改为ArrayList,测试表明这种方式更快  
    1. 当不检测循环引用的时候,当对象层级超过30将会抛出异常  
    1. IJsonObject 增加属性 Object Value { get; }  
    1. 增加JsonType 和 JsonMember 两个对象,用于缓存C#和Json对象转换的相关信息  
    1. 优化JsonParser中大部分代码逻辑,更清晰和高效  
    1. 增加StringConverter用于将String转换为任意对象  
    1. 减少GC的回收消耗  
    1. 修改其他代码和bug  
* 增加对特性的支持,现有3个特性JsonFormatAttribute(自定义格式化),JsonIgnoreAttribute(忽略),JsonNameAttribute(自定义json名称)  
* 增加序列化参数 JsonBuilderSettings,包括 时间/布尔/枚举的默认格式,是否序列化字段,数字/布尔是否加引号,是否检测循环引用,是否忽略null值等  
* 修改DataTable对象的默认Json格式为常用格式  

#### 2014.07.29  
* 新增 IJsonObject 类型  

```csharp
User user = User.TestUser();
IJsonObject jobj = Json.ToJsonObject(Json.ToJsonString(user));
Console.WriteLine(user.Name == jobj["Name"].ToString());
Console.WriteLine(user.LoginHistory[0] == Convert.ToDateTime(jobj["LoginHistory"][0]));
Console.WriteLine(user.Info.Phone["手机"] == jobj["Info"]["Phone"]["手机"].ToString());
```

#### 2014.07.24  
* 优化性能  
* 优化代码结构
* 修正AppendChar方法中的BUG  
* 增加对特殊字符 \a,\b,\v,\f 的处理  

#### 2014.07.23  
* 修正几处关于时间类型的已知BUG  
* 增加单元测试,测试数据与Newtonsoft对比  
* 版本号:1.14.0723  
* 接下来准备2.x版本的开发工作  
* 预计新功能有  支持dynamic,支持Attribute,支持几种预定格式切换,新增JsonObject对象