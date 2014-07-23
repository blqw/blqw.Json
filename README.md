Json序列化与反序列化  
http://cnblogs.com/blqw/p/json.html  
使用方便 ,性能卓越  
```csharp
blqw.Json.ToJsonString(object);
blqw.Json.ToObject<T>(string);
blqw.Json.ToObject(string);
blqw.Json.ToObject(Type,string);
```

####更新说明
* 2014.07.23  
修正几处关于时间类型的已知BUG  
增加单元测试,测试数据与Newtonsoft对比  
版本号:1.14.0723  
接下来准备2.x版本的开发工作  
预计新功能有  支持dynamic,支持Attribute,支持几种预定格式切换,新增JsonObject对象