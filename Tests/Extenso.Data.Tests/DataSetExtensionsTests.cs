using System.Data;
using System.Globalization;
using System.Text;
using Bogus;
using CsvHelper;
using CsvHelper.Configuration;
using DataSet = System.Data.DataSet;

namespace Extenso.Data.Tests
{
    public class DataSetExtensionsTests
    {
        [Fact]
        public void ToDelimited()
        {
            var dataSet = new DataSet();

            #region People

            var peopleTable = new DataTable("People");
            peopleTable.Columns.AddRange("FirstName", "LastName", "Order");

            var personFaker = new Faker<Person>()
                .RuleFor(x => x.FirstName, x => x.Name.FirstName())
                .RuleFor(x => x.LastName, x => x.Name.LastName());

            var people = new List<Person>();
            for (int i = 0; i < 10; i++)
            {
                var person = personFaker.Generate();
                person.Order = i;
                people.Add(person);

                var row = peopleTable.NewRow();
                row.SetField("FirstName", person.FirstName);
                row.SetField("LastName", person.LastName);
                row.SetField("Order", person.Order);
                peopleTable.Rows.Add(row);
            }

            dataSet.Tables.Add(peopleTable);

            #endregion People

            #region Vehicles

            var vehiclesTable = new DataTable("Vehicles");
            vehiclesTable.Columns.AddRange("Name", "Order");

            var vehicleFaker = new Faker<Vehicle>()
                .RuleFor(x => x.Name, x => x.Vehicle.Manufacturer());

            var vehicles = new List<Vehicle>();
            for (int i = 0; i < 10; i++)
            {
                var vehicle = vehicleFaker.Generate();
                vehicle.Order = i;
                vehicles.Add(vehicle);

                var row = vehiclesTable.NewRow();
                row.SetField("Name", vehicle.Name);
                row.SetField("Order", vehicle.Order);
                vehiclesTable.Rows.Add(row);
            }

            dataSet.Tables.Add(vehiclesTable);

            #endregion Vehicles

            var csv = dataSet.ToDelimited();

            Assert.NotNull(csv);
            Assert.NotEmpty(csv);
            Assert.True(csv.Count == 2);

            string expected = CollectionToCsv(people);
            Assert.Equal(expected, csv[0]);

            expected = CollectionToCsv(vehicles);
            Assert.Equal(expected, csv[1]);
        }

        private string CollectionToCsv<T>(IEnumerable<T> collection)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);
            using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture) { ShouldQuote = args => true }))
            {
                csvWriter.WriteRecords(collection);
            }

            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        private class Person
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public int Order { get; set; }
        }

        private class Vehicle
        {
            public string Name { get; set; }

            public int Order { get; set; }
        }
    }
}