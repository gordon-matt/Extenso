using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Extenso.Data.Entity
{
    public static class DatabaseFacadeExtensions
    {
        public static string GenerateCreateScript(this DatabaseFacade database)
        {
            var model = database.GetService<IModel>();
            var migrationsModelDiffer = database.GetService<IMigrationsModelDiffer>();
            var migrationsSqlGenerator = database.GetService<IMigrationsSqlGenerator>();
            var sqlGenerationHelper = database.GetService<ISqlGenerationHelper>();

            var operations = migrationsModelDiffer.GetDifferences(null, model);
            var commands = migrationsSqlGenerator.Generate(operations, model);

            var stringBuilder = new StringBuilder();
            foreach (var command in commands)
            {
                stringBuilder
                    .Append(command.CommandText)
                    .AppendLine(sqlGenerationHelper.BatchTerminator);
            }

            return stringBuilder.ToString();
        }
    }
}