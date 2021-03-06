﻿using System;

namespace blqw
{
    /// <summary>
    /// Json序列化的设置
    /// </summary>
    [Flags, Serializable]
    public enum JsonBuilderSettings
    {
        /// <summary>
        /// 不使用任何设置
        /// </summary>
        None = 0,

        /// <summary>
        /// 格式化 DateTime 对象中的日期
        /// </summary>
        FormatDate = 1 << 1,

        /// <summary>
        /// 格式化 DateTime 对象中的时间
        /// </summary>
        FormatTime = 1 << 2,

        /// <summary>
        /// 同时序列化字段
        /// </summary>
        SerializableField = 1 << 3,

        /// <summary>
        /// 使用双引号包装数字的值
        /// </summary>
        QuotWrapNumber = 1 << 4,

        /// <summary>
        /// 将布尔值转为数字值 true = 1 ,false = 0
        /// </summary>
        BooleanToNumber = 1 << 5,

        /// <summary>
        /// 将枚举转为对应的数字值
        /// </summary>
        EnumToNumber = 1 << 6,

        /// <summary>
        /// 检查循环引用,发现循环应用时输出 undefined
        /// </summary>
        CheckLoopRef = 1 << 7,

        /// <summary>
        /// 格式化 DateTime 对象中的时间时忽略(00:00:00.000000) ,存在FormatTime时才生效
        /// </summary>
        IgnoreEmptyTime = 1 << 8,

        /// <summary>
        /// 使用双引号包装布尔的值
        /// </summary>
        QuotWrapBoolean = 1 << 9,

        /// <summary>
        /// 忽略值是null的属性
        /// </summary>
        IgnoreNullMember = 1 << 10,

        /// <summary>
        /// 默认序列化设置 FormatDate | FormatTime | IgnoreEmptyTime | IgnoreNullMember
        /// </summary>
        Default = FormatDate | FormatTime | IgnoreEmptyTime | IgnoreNullMember | EnumToNumber | GuidHasHyphens,

        /// <summary>
        /// 输出类型信息
        /// </summary>
        SerializableType = 1 << 11,

        /// <summary>
        /// 输出所有属性/字段
        /// </summary>
        FormatAllMember = 1 << 12,

        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        FilterSpecialCharacter = 1 << 13,

        /// <summary>
        /// Guid包含连字符
        /// </summary>
        GuidHasHyphens = 1 << 14,

        /// <summary>
        /// Guid需要转为大写
        /// </summary>
        GuidToUpper = 1 << 15,

        /// <summary>
        /// 是否将大于255的字符转为Unicode编码
        /// </summary>
        CastUnicode = 1 << 16
    }
}