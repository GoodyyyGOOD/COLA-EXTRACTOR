using System.Threading.Tasks;

namespace Sofos2toDatawarehouseSyncer.Web.Abstractions
{
    public interface IViewRenderService
    {
        Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
    }
}