using CsvHelper;
using CustomerInArrears.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CustomerInArrears.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CsvFileController : ControllerBase
    {

        [HttpGet("getcustomerinfo")]
        public IEnumerable<CustomerData> GetData()
        {
            var streamReader = new StreamReader(@"C:\Users\Kamrul Haque\Desktop\Small Application ExampleData.csv");

            var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            var customerData = csvReader.GetRecords<CustomerData>().ToList();

            return customerData;
        }


        [HttpPost("uploadcsvfile")]
        public async Task<IEnumerable<CustomerData>> UploadCsvFile(IFormFile file)
        {
            var streamReader = new StreamReader(file.OpenReadStream());
            var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            var customerData = csvReader.GetRecords<CustomerData>().ToList();

            return customerData;
        }


    }
}
