using EntityFrameworkCore.TemporalTables.Sql.Table;

namespace EntityFrameworkCore.TemporalTables.Tests.Mocks
{
    public class FakeTableHelperNonTemporal : ITableHelper<FakeDataContext>
    {
        public bool IsTableTemporal(string tableName, string schema = "dbo") => false;
    }
}
