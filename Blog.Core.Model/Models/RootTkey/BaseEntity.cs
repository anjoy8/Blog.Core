using Blog.Core.Model.Models.RootTkey.Interface;
using SqlSugar;
using System;

namespace Blog.Core.Model.Models.RootTkey;

public class BaseEntity : RootEntityTkey<long>, IDeleteFilter
{
    #region 数据状态管理

    /// <summary>
    /// 状态 <br/>
    /// 中立字段，某些表可使用某些表不使用
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 中立字段，某些表可使用某些表不使用   <br/>
    /// 逻辑上的删除，非物理删除  <br/>
    /// 例如：单据删除并非直接删除
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 中立字段 <br/>
    /// 是否内置数据
    /// </summary>
    public bool IsInternal { get; set; }

    #endregion

    #region 创建

    /// <summary>
    /// 创建ID
    /// </summary>
    [SugarColumn(IsNullable = true, IsOnlyIgnoreUpdate = true)]
    public long? CreateId { get; set; }

    /// <summary>
    /// 创建者
    /// </summary>
    [SugarColumn(IsNullable = true, IsOnlyIgnoreUpdate = true)]
    public string CreateBy { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(IsOnlyIgnoreUpdate = true)]
    public DateTime CreateTime { get; set; } = DateTime.Now;

    #endregion

    #region 修改

    /// <summary>
    /// 修改ID
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public long? ModifyId { get; set; }

    /// <summary>
    /// 更新者
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string ModifyBy { get; set; }

    /// <summary>
    /// 修改日期
    /// </summary>
    public DateTime? ModifyTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 数据版本
    /// </summary>
    [SugarColumn(DefaultValue = "0", IsEnableUpdateVersionValidation = true)] //标识版本字段
    public long Version { get; set; }

    #endregion
}