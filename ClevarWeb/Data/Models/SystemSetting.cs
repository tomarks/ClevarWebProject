using ClevarWeb.Data.Models.Internals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClevarWeb.Data.Models
{
    public class SystemSetting
    {
        #region PROPS
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [MaxLength(255, ErrorMessage = "Exceeds max length of 255 characters!")]
        [Display(Name = "Website Title Tag")]
        public string SiteTitle { get; set; }

        [MaxLength(4000, ErrorMessage = "Exceeds max length of 4000 characters!")]
        [Display(Name = "Website Keywords Meta Tag")]
        public string SiteKeywords { get; set; }

        [MaxLength(4000, ErrorMessage = "Exceeds max length of 4000 characters!")]
        [Display(Name = "Website Description Meta Tag")]
        public string SiteDescription { get; set; }

        [MaxLength(255)]
        [Display(Name = "Twitter URL")]
        [RegularExpression(@"^https?:\/\/.*", ErrorMessage = "Invalid URL - Must start with http:// or https://")]
        public string SocialTwitterURL { get; set; }

        [MaxLength(255)]
        [Display(Name = "Facebook URL")]
        [RegularExpression(@"^https?:\/\/.*", ErrorMessage = "Invalid URL - Must start with http:// or https://")]
        public string SocialFacebookURL { get; set; }

        [MaxLength(255)]
        [Display(Name = "Instagram URL")]
        [RegularExpression(@"^https?:\/\/.*", ErrorMessage = "Invalid URL - Must start with http:// or https://")]
        public string SocialInstagramURL { get; set; }

        [MaxLength(255)]
        [Display(Name = "Linkedin URL")]
        [RegularExpression(@"^https?:\/\/.*", ErrorMessage = "Invalid URL - Must start with http:// or https://")]
        public string SocialLinkedinURL { get; set; }

        [MaxLength(255)]
        [Display(Name = "Public Email Address")]
        [EmailAddress]
        public string PublicEmailAddress { get; set; }

        List<Document> HeaderSlideDocuments { get; set; } = new List<Document>();

        #endregion PROPS

        #region STATIC

        public static SystemSetting GetDefaultSystemSetting()
        {
            var defaultSystemSettings = new SystemSetting()
            {
                SiteTitle = "CLEVAR",
                PublicEmailAddress = "",
                SiteDescription = "VR & AR Research Group",
                SiteKeywords = "vr,ar,virtual,augmented,reality"
            };
            return defaultSystemSettings;
        }

        #endregion STATIC

    }
}
