using fomin_server.http;

namespace fomin_server.core
{
    public interface IMiddleware<in TReq, in TResp>
        where TReq : IRequest
        where TResp : IResponse
    {
        void Render(TReq request, TResp response);
    }
}
