namespace Cyberbezpieczenstwo.API.Models
{
    public class SecuritySettingsDTO: CreateSecuritySettingsDTO
    {
        
    public SecuritySettingsDTO(int id,bool isSetLimitOfMaxFailedLoginAttemps,int? maxNumbersOfFailedLoginAttemps,bool isSetAutoLogout,  int? secToAutoLogout, bool captchaEnabled, bool reCaptchaEnabled) : 
            base(isSetLimitOfMaxFailedLoginAttemps, maxNumbersOfFailedLoginAttemps, isSetAutoLogout, secToAutoLogout, captchaEnabled, reCaptchaEnabled)
        {
            Id = id;
        }
        public SecuritySettingsDTO() { }

        public int Id { get; set; }
    }

    public abstract class CreateSecuritySettingsDTO
    {
        public CreateSecuritySettingsDTO(bool isSetLimitOfMaxFailedLoginAttemps, int? maxNumbersOfFailedLoginAttemps
            ,bool isSetAutoLogout, int? secToAutoLogout, bool captchaEnabled, bool reCaptchaEnabled)
        {
            IsSetLimitOfMaxFailedLoginAttemps = isSetLimitOfMaxFailedLoginAttemps;
            MaxNumbersOfFailedLoginAttemps = maxNumbersOfFailedLoginAttemps;
            IsSetAutoLogout = isSetAutoLogout;
            SecToAutoLogout = SecToAutoLogout;
            CaptchaEnabled = captchaEnabled;
            ReCaptchaEnabled = reCaptchaEnabled;
        }
        public CreateSecuritySettingsDTO(){}
        public bool IsSetLimitOfMaxFailedLoginAttemps { get; set; }
        public int? MaxNumbersOfFailedLoginAttemps { get; set; }
        public int? SecToAutoLogout { get; set; }
        public bool IsSetAutoLogout { get; set; }
        public bool CaptchaEnabled { get; set; }
        public bool ReCaptchaEnabled { get; set; }
    }
}
