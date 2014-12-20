# blqw.Json 最快的Json序列化类库
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
## 性能比较

#### 序列化测试
> ps:blqw.Json 的序列化器分为 JsonBuilder和QuickJsonBuilder 2个版本   
> JsonBuilder 是纯反射实现  
> QuickJsonBuilder 是使用[blqw.Literacy](https://coding.net/u/blqw/p/blqw-Literacy/git)代替反射实现(由此Literacy性能也可以见一斑)  
> 以下测试中 blqw.Json 的就表示使用 QuickJsonBuilder 序列化器完成Json序列化的性能  

<table style="width: 660px; float: left; height: 354px;" border="1" cellpadding="2"><caption>&nbsp;</caption>
<tbody>
<tr>
<td>循环次数</td>
<td>测试组件</td>
<td>第一轮</td>
<td>第二轮</td>
<td>第三轮</td>
<td>第四轮</td>
<td>第五轮</td>
</tr>
<tr style="background-color: #ffffcc;">
<td rowspan="7">1次</td>
<td>&nbsp;JavaScriptSerializer</td>
<td>&nbsp;<span style="color: #ff0000;"><span style="color: #339966;">12</span><br></span></td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
</tr>
<tr style="background-color: #ffffcc;">
<td>&nbsp;Newtonsoft.Json</td>
<td>&nbsp;<span style="color: #ff0000;">208</span></td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
</tr>
<tr style="background-color: #ffffcc;">
<td>&nbsp;Jayrock.Json</td>
<td>&nbsp;85</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
</tr>
<tr style="background-color: #ffffcc;">
<td>&nbsp;fastJSON.NET</td>
<td>&nbsp;47</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
</tr>
<tr style="background-color: #ffffcc;">
<td>&nbsp;blqw.Json</td>
<td>&nbsp;37</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
</tr>
<tr style="background-color: #ffffcc;">
<td>&nbsp;JsonBuilder</td>
<td>&nbsp;22</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
</tr>
<tr style="background-color: #ffffcc;">
<td>ServiceStack.Text</td>
<td>&nbsp;138</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
<td>&nbsp;0</td>
</tr>
<tr style="background-color: #ffff99;">
<td rowspan="7">100次</td>
<td>&nbsp;<span>JavaScriptSerializer</span></td>
<td>&nbsp;<span style="color: #339966;">23</span></td>
<td>&nbsp;7</td>
<td>&nbsp;7</td>
<td>&nbsp;8</td>
<td>&nbsp;7</td>
</tr>
<tr style="background-color: #ffff99;">
<td>&nbsp;<span>Newtonsoft.Json</span></td>
<td>&nbsp;<span style="color: #ff0000;">201</span></td>
<td>&nbsp;2</td>
<td>&nbsp;3</td>
<td>&nbsp;2</td>
<td>&nbsp;2</td>
</tr>
<tr style="background-color: #ffff99;">
<td>&nbsp;<span>Jayrock.Json</span></td>
<td>&nbsp;77</td>
<td><span style="color: #ff0000;">&nbsp;8</span></td>
<td><span style="color: #ff0000;">&nbsp;9</span></td>
<td><span style="color: #ff0000;">&nbsp;9</span></td>
<td><span style="color: #ff0000;">&nbsp;8</span></td>
</tr>
<tr style="background-color: #ffff99;">
<td>&nbsp;<span>fastJSON.NET</span></td>
<td>&nbsp;41</td>
<td><span style="color: #339966;">&nbsp;1</span></td>
<td><span style="color: #339966;">&nbsp;1</span></td>
<td><span style="color: #339966;">&nbsp;1</span></td>
<td><span style="color: #339966;">&nbsp;1</span></td>
</tr>
<tr style="background-color: #ffff99;">
<td>&nbsp;<span>blqw.Json</span></td>
<td>&nbsp;36</td>
<td><span style="color: #339966;">&nbsp;1</span></td>
<td><span style="color: #339966;">&nbsp;1</span></td>
<td><span style="color: #339966;">&nbsp;1</span></td>
<td><span style="color: #339966;">&nbsp;1</span></td>
</tr>
<tr style="background-color: #ffff99;">
<td>&nbsp;JsonBuilder</td>
<td>&nbsp;26</td>
<td>&nbsp;3</td>
<td>&nbsp;4</td>
<td>&nbsp;3</td>
<td>&nbsp;3</td>
</tr>
<tr style="background-color: #ffff99;">
<td>ServiceStack.Text</td>
<td>&nbsp;139</td>
<td>&nbsp;2</td>
<td>&nbsp;2</td>
<td>&nbsp;2</td>
<td>&nbsp;2</td>
</tr>
<tr style="background-color: #ffe4c4;">
<td rowspan="7">10000次</td>
<td>&nbsp;<span>JavaScriptSerializer</span></td>
<td>&nbsp;765</td>
<td>&nbsp;751</td>
<td>&nbsp;752</td>
<td>&nbsp;751</td>
<td>&nbsp;749</td>
</tr>
<tr style="background-color: #ffe4c4;">
<td>&nbsp;<span>Newtonsoft.Json</span></td>
<td>&nbsp;437</td>
<td>&nbsp;253</td>
<td>&nbsp;251</td>
<td>&nbsp;248</td>
<td>&nbsp;243</td>
</tr>
<tr style="background-color: #ffe4c4;">
<td>&nbsp;<span>Jayrock.Json</span></td>
<td><span style="color: #ff0000;">&nbsp;967</span></td>
<td><span style="color: #ff0000;">&nbsp;905</span></td>
<td><span style="color: #ff0000;">&nbsp;965</span></td>
<td><span style="color: #ff0000;">&nbsp;913</span></td>
<td><span style="color: #ff0000;">&nbsp;952</span></td>
</tr>
<tr style="background-color: #ffe4c4;">
<td><span>&nbsp;</span><span>fastJSON.NET</span></td>
<td>&nbsp;239</td>
<td>&nbsp;181</td>
<td>&nbsp;200</td>
<td>&nbsp;167</td>
<td>&nbsp;166</td>
</tr>
<tr style="background-color: #ffe4c4;">
<td>&nbsp;<span>blqw.Json</span></td>
<td><span style="color: #339966;">&nbsp;171</span></td>
<td><span style="color: #339966;">&nbsp;128</span></td>
<td><span style="color: #339966;">&nbsp;132</span></td>
<td><span style="color: #339966;">&nbsp;136</span></td>
<td><span style="color: #339966;">&nbsp;129</span></td>
</tr>
<tr style="background-color: #ffe4c4;">
<td>&nbsp;JsonBuilder</td>
<td>&nbsp;418</td>
<td>&nbsp;386</td>
<td>&nbsp;388</td>
<td>&nbsp;391</td>
<td>&nbsp;360</td></tr>
<tr style="background-color: #ffe4c4;">
<td>ServiceStack.Text</td>
<td>&nbsp;367</td>
<td>&nbsp;216</td>
<td>&nbsp;224</td>
<td>&nbsp;238</td>
<td>&nbsp;223</td>
</tr>
</tbody>
</table>
<div style="clear:both"></div>
#### 与号称最快的FastJson比较
    blqw.Json序列化 Demo.User 循环 500000 次
     运行时间    CPU时钟周期    垃圾回收( 1代      2代      3代 )
     3,824ms     7,596,991,952            243      1        0

    FastJson序列化 Demo.User 循环 500000 次
     运行时间    CPU时钟周期    垃圾回收( 1代      2代      3代 )
     7,451ms     14,845,112,542           747      0        0

    ========================
    blqw.Json序列化 Demo.ResultDTO 循环 10000 次
     运行时间    CPU时钟周期    垃圾回收( 1代      2代      3代 )
     2,528ms     5,026,517,462            322      0        0

    FastJson序列化 Demo.ResultDTO 循环 10000 次
     运行时间    CPU时钟周期    垃圾回收( 1代      2代      3代 )
     5,491ms     10,919,771,327           599      0        0

    ========================
    blqw.Json序列化 List<Demo.Object2> 循环 1250 次
     运行时间    CPU时钟周期    垃圾回收( 1代      2代      3代 )
     2,461ms     4,908,125,075            357      357      357

    FastJson序列化 List<Demo.Object2> 循环 1250 次
     运行时间    CPU时钟周期    垃圾回收( 1代      2代      3代 )
     4,139ms     8,143,468,287            231      116      113

    ========================
    blqw.Json反序列化 List<Demo.Object2> 循环 25 次
     运行时间    CPU时钟周期    垃圾回收( 1代      2代      3代 )
     226ms       442,187,752              9        0        0

    FastJson反序列化 List<Demo.Object2> 循环 25 次
     运行时间    CPU时钟周期    垃圾回收( 1代      2代      3代 )
     543ms       1,030,657,833            33       21       10

##更新说明
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
* 同步更新 [blqw.Literacy](https://coding.net/u/blqw/p/blqw-Literacy/git)

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