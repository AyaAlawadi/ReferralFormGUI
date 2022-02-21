namespace ReferralFormGUI.Services
{
    public interface INationalClient
    {
        Task<TResp> Get<TReq, TResp>(NationalWebApiRequest<TReq> request);
    }
}
