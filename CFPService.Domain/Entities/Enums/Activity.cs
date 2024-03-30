using System.ComponentModel;

namespace CFPService.Domain.Entities.Enums
{
    public enum Activity
    {
        [Description("Доклад, 35-45 минут")]
        Report = 0,

        [Description("Мастеркласс, 1-2 часа")]
        MasterClass = 1,

        [Description("Дискуссия / круглый стол, 40-50 минут")]
        Discussion = 2
    }
}
