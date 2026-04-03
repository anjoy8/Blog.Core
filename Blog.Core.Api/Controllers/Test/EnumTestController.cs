using System.ComponentModel;
using Blog.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Api.Controllers.Test;

/// <summary>
/// 枚举测试
/// </summary>
[Route("api/[Controller]/[Action]")]
[AllowAnonymous]
public class EnumTestController : BaseApiController
{
    /// <summary>
    /// 获取学生信息
    /// </summary>
    /// <param name="studentType">学生类型</param>
    /// <param name="studentType2"></param>
    /// <param name="studentTypes"></param>
    /// <returns>学生信息</returns>
    [HttpGet]
    public Student GetStudent( StudentType studentType, StudentType? studentType2,  List<StudentType> studentTypes)
    {
        return new Student
        {
            Name = "张三",
            Age  = 20,
            Type = studentType
        };
    }
}

/// <summary>
/// 学生类型
/// </summary>
[Description("学生类型")]
public enum StudentType
{
    /// <summary>
    /// 小学生
    /// </summary>
    [Description("小学生")]
    PrimarySchool = 1,

    /// <summary>
    /// 中学生
    /// </summary>
    [Description("中学生")]
    MiddleSchool = 2,

    /// <summary>
    /// 大学生
    /// </summary>
    [Description("大学生")]
    University = 3
}

public class Student
{
    /// <summary>
    /// 学生姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 学生年龄
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// 学生类型
    /// </summary>
    public StudentType Type { get; set; }
}