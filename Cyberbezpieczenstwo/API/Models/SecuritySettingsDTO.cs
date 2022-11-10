namespace Cyberbezpieczenstwo.API.Models
{
    public class SecuritySettingsDTO: CreateSecuritySettingsDTO
    {
        
    public SecuritySettingsDTO(int id,bool isSetLimitOfMaxFailedLoginAttemps,int? maxNumbersOfFailedLoginAttemps,bool isSetAutoLogout,  int? secToAutoLogout) : 
            base(isSetLimitOfMaxFailedLoginAttemps, maxNumbersOfFailedLoginAttemps, isSetAutoLogout, secToAutoLogout)
        {
            Id = id;
        }
        public SecuritySettingsDTO() { }

        public int Id { get; set; }
    }

    public abstract class CreateSecuritySettingsDTO
    {
        public CreateSecuritySettingsDTO(bool isSetLimitOfMaxFailedLoginAttemps, int? maxNumbersOfFailedLoginAttemps
            ,bool isSetAutoLogout, int? secToAutoLogout)
        {
            IsSetLimitOfMaxFailedLoginAttemps = isSetLimitOfMaxFailedLoginAttemps;
            MaxNumbersOfFailedLoginAttemps = maxNumbersOfFailedLoginAttemps;
            IsSetAutoLogout = isSetAutoLogout;
            SecToAutoLogout = SecToAutoLogout;
        }
        public CreateSecuritySettingsDTO(){}
        public bool IsSetLimitOfMaxFailedLoginAttemps { get; set; }
        public int? MaxNumbersOfFailedLoginAttemps { get; set; }
        public int? SecToAutoLogout { get; set; }
        public bool IsSetAutoLogout { get; set; }
    }
}
