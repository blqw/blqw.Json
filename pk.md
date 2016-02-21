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


#### 与FastJson比较
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
