using SqlSugar;
using System.Linq.Expressions;
namespace DataConCore.TableEntitys;

public abstract class BaseEntity : IAppCore
{
    public DateTime CreateTime { get; set; }

    public int IsDel { get; set; }
}
