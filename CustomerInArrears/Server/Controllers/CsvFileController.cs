using CsvHelper;
using CustomerInArrears.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.Http.Headers;

namespace CustomerInArrears.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CsvFileController : ControllerBase
    {
   
        [HttpPost("uploadcsvfile")]
        public async Task<IEnumerable<CustomerData>> UploadedCsvFile(IFormFile file)
        {
            var streamReader = new StreamReader(file.OpenReadStream());
            var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            var customerData = csvReader.GetRecords<CustomerData>().ToList();

            return customerData;
        }

    }

}
