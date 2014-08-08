Json序列化与反序列化  
http://cnblogs.com/blqw/p/json.html  
使用方便 ,性能卓越  
```csharp
blqw.Json.ToJsonString(object);
blqw.Json.ToObject<T>(string);
blqw.Json.ToObject(string);
blqw.Json.ToObject(Type,string);
//2014.07.29新增
blqw.Json.ToJsonObject(string);
//2014.08.08新增
blqw.Json.ToJsonString(object,JsonBuilderSettings);
```

##更新说明
* 2014.08.08   

#### 2.0b1 版本发布  

优化代码 :  

> 1. 放弃直接使用JsonBuilder ,现在JsonBuilder仅作为抽象基类存在  
> 1. 放弃使用Dictionary检测循环引用,改为ArrayList,测试表明这种方式更快  
> 1. 当不检测循环引用的时候,当对象层级超过30将会抛出异常  
> 1. IJsonObject 增加属性 Object Value { get; }  
> 1. 增加JsonType 和 JsonMember 两个对象,用于缓存C#和Json对象转换的相关信息  
> 1. 优化JsonParser中大部分代码逻辑,更清晰和高效  
> 1. 增加StringConverter用于将String转换为任意对象  
> 1. 减少GC的回收消耗  
> 1. 修改其他代码和bug  

增加对特性的支持,现有3个特性JsonFormatAttribute(自定义格式化),JsonIgnoreAttribute(忽略),JsonNameAttribute(自定义json名称)  
增加序列化参数 JsonBuilderSettings,包括 时间/布尔/枚举的默认格式,是否序列化字段,数字/布尔是否加引号,是否检测循环引用,是否忽略null值等  
修改DataTable对象的默认Json格式为常用格式  

* 2014.07.29  
新增 IJsonObject 类型  
```csharp
User user = User.TestUser();
IJsonObject jobj = Json.ToJsonObject(Json.ToJsonString(user));
Console.WriteLine(user.Name == jobj["Name"].ToString());
Console.WriteLine(user.LoginHistory[0] == Convert.ToDateTime(jobj["LoginHistory"][0]));
Console.WriteLine(user.Info.Phone["手机"] == jobj["Info"]["Phone"]["手机"].ToString());
```

* 2014.07.24  
优化性能  
优化代码结构
修正AppendChar方法中的BUG  
增加对特殊字符 \a,\b,\v,\f 的处理  
  
* 2014.07.23  
修正几处关于时间类型的已知BUG  
增加单元测试,测试数据与Newtonsoft对比  
版本号:1.14.0723  
接下来准备2.x版本的开发工作  
预计新功能有  支持dynamic,支持Attribute,支持几种预定格式切换,新增JsonObject对象