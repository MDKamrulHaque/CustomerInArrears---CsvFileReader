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
        public async Task<IEnumerable<CustomerData>> UploadCsvFile(IFormFile file)
        {
            var streamReader = new StreamReader(file.OpenReadStream());
            var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            var customerData = csvReader.GetRecords<CustomerData>().ToList();

            return customerData;
        }

        [HttpPost("exportFile")]
        public async Task<string> ExportFilteredCsvFile(IEnumerable<CustomerData> filteredData)
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);
            var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            foreach (var customerData in filteredData)
            {
                await streamWriter.WriteLineAsync($"1, Hello {customerData.ClientName} - you are receiving this message as you are currently in arrears., MoblieNumber: {customerData.MobileNumber}, TenencyNumber: {customerData.TenancyNumber},{DateTime.Now:dd/MM/yyyy H:mm}");
            }
            
            memoryStream.Seek(0, SeekOrigin.Begin);

            var content = new StreamContent(memoryStream);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");           
            var exportUrl = $"data:text/csv;charset=utf-8,{Uri.EscapeDataString(await content.ReadAsStringAsync())}";

            return exportUrl;
        }

    }

}
