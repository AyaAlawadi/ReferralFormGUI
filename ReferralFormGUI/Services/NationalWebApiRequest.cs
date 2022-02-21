using NPOI.SS.Formula.Functions;

namespace ReferralFormGUI.Services
{
    public class NationalWebApiRequest<TReq>
    {
        public string Endpoint;
        public List<T> RequestBody;
    }
}