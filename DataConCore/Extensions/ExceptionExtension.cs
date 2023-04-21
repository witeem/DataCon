using System.Text;
public static class ExceptionExtension
{
    public static string GetExceptionDetail(this Exception exception)
    {
        var detail = new StringBuilder();
        detail.Append(@"***************************************");
        detail.AppendFormat(@" Exception occurrence time： {0} ", DateTime.Now);
        detail.AppendFormat(@" Types of exceptions： {0} ", exception.HResult);
        detail.AppendFormat(@" The Exception instance that caused the current exception： {0} ", exception.InnerException);
        detail.AppendFormat(@" The name of the application or object that caused the exception： {0} ", exception.Source);
        detail.AppendFormat(@" A method that raises an exception： {0} ", exception.TargetSite);
        detail.AppendFormat(@" Exception stack information： {0} ", exception.StackTrace);
        detail.AppendFormat(@" Exception message： {0} ", exception.Message);
        detail.Append(@"***************************************");

        return detail.ToString();
    }
}
