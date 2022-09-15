using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmailSenderApplication.Models
{
    public class Email 
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public string Recipient { get ; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Text { get; set; }

        [JsonIgnore]
        public bool IsSuccessfulSend { get; set; }

        [JsonPropertyName("carbon_copy_recipients")]
        public List<string>? CarbonCopyRecipients { get; set; }
    }
}
