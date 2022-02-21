using Newtonsoft.Json;


namespace ReferralFormGUI.Models
{
    public class InitiativeApiResponse
    {
        [JsonProperty("value")]
        public List<InitiativeApiResponse>? initiatives { get; set; }
    }
}
