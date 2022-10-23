using Guardian.Application.DTO;

namespace Guardian.Data
{
    public class GuardianData
    {
        public static List<GuardianDTO> guardianList = new List<GuardianDTO>
        {
            new GuardianDTO{Id=1, Name="Felippe Delesporte"},
            new GuardianDTO{Id=2, Name="Enrico Delesporte"}
        };
    }
}
