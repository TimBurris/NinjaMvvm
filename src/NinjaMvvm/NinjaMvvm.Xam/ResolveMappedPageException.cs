using System;

namespace NinjaMvvm.Xam
{
    public class ResolveMappedPageException : ApplicationException
    {
        public ResolveMappedPageException()
        {
        }
        public ResolveMappedPageException(Abstractions.IPageModel pageModel)
        {
            this.PageModel = pageModel;
        }
        public ResolveMappedPageException(Abstractions.IPageModel pageModel, Exception innerException)
            : base(message: "", innerException)
        {
            this.PageModel = pageModel;
        }
        public Abstractions.IPageModel PageModel { get; set; }
        public override string Message => $"Error resolving page for PageModel type {this.PageModel.GetType()}.  Ensure you have declared a page that implements IPage<T>.  If IPage<T> is implemented, check InnerExpception for more information";
    }
}
