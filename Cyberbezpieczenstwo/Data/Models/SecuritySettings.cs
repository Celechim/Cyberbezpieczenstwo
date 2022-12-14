using Cyberbezpieczenstwo.SharedKernel;
using Cyberbezpieczenstwo.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cyberbezpieczenstwo.Data.Models
{
    public class SecuritySettings : EntityBase, IAggregateRoot
    {
        //int NumberOfMinutesBeforeLogut { get; set; }
        public bool IsSetLimitOfMaxFailedLoginAttemps { get; set; }
        public bool IsSetAutoLogout { get; set; }
        public bool CaptchaEnabled { get; set; }
        public bool ReCaptchaEnabled { get; set; }
        public int? MaxNumbersOfFailedLoginAttemps { get; set; }
        public int? SecToAutoLogout { get; set; }

        public SecuritySettings()
        {

        }
        public SecuritySettings(int id,
            bool isSetLimitOfMaxFailedLoginAttemps,
            int? maxNumbersOfFailedLoginAttemps,
            bool isSetAutoLogout,
            int? secToAutoLogout,
            bool captchaEnabled,
            bool reCaptchaEnabled)
        {
            Id = id;
            IsSetLimitOfMaxFailedLoginAttemps = isSetLimitOfMaxFailedLoginAttemps;
            MaxNumbersOfFailedLoginAttemps = maxNumbersOfFailedLoginAttemps;
            IsSetAutoLogout = isSetAutoLogout;
            SecToAutoLogout = secToAutoLogout;
            CaptchaEnabled = captchaEnabled;
            ReCaptchaEnabled = reCaptchaEnabled;
        }
    }
}
