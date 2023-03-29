using CsvHelper;
using CustomerInArrears.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using CustomerInArrears.Shared;
using CsvHelper;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using System.IO;

namespace CustomerInArrears.Client.Pages
{
    public partial class CustomerInfoPage
    {
        [Inject] HttpClient Http { get; set; }

        bool fixedheader = true;
        private List<CustomerData>? customerDatas;
        private string exportUrl;
        private string exportFileName;

        private async Task HandleFileSelected(InputFileChangeEventArgs e) 
        {
            var file = e.File; 
            var formData = new MultipartFormDataContent();  
            formData.Add(new StreamContent(file.OpenReadStream()), "file", file.Name);                 

            var response = await Http.PostAsync("api/CsvFile/uploadcsvfile", formData); 

            if (response.IsSuccessStatusCode)
            {
                customerDatas = await response.Content.ReadFromJsonAsync<List<CustomerData>>(); 
            }
        }

        //private async Task ExportFilteredCsvFile()
        //{
        //    var filteredData = customerDatas?.Where(cd => cd.TenancyBalance < 0 && cd.MobileNumber.Length == 11 && cd.MobileNumber.StartsWith("07"));  

        //    var memoryStream = new MemoryStream(); 
        //    var streamWriter = new StreamWriter(memoryStream); 
        //    var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture); 


        //    foreach (var customerData in filteredData) 
        //    {
        //        await streamWriter.WriteLineAsync($"1, Hello {customerData.ClientName} - you are receiving this message as you are currently in arrears., MoblieNumber: {customerData.MobileNumber}, TenencyNumber: {customerData.TenancyNumber},{DateTime.Now:dd/MM/yyyy H:mm}");

        //    }
        //    await streamWriter.FlushAsync(); 
        //    memoryStream.Seek(0, SeekOrigin.Begin); 

        //    var content = new StreamContent(memoryStream); 
        //    content.Headers.ContentType = new MediaTypeHeaderValue("text/csv"); 
        //    exportFileName = $"exportedCsv-{DateTime.Now.ToFileTime()}.csv"; 
        //    exportUrl = $"data:text/csv;charset=utf-8,{Uri.EscapeDataString(await content.ReadAsStringAsync())}"; 
        //}

        private async Task ExportFilteredCsvFile()
        {
            var filteredData = customerDatas?.Where(cd => cd.TenancyBalance < 0 && cd.MobileNumber.Length == 11 && cd.MobileNumber.StartsWith("07"));

            var response = await Http.PostAsJsonAsync("api/CsvFile/exportFile", filteredData);
            if (response.IsSuccessStatusCode)
            {
                var memoryStream = new MemoryStream();
                var streamWriter = new StreamWriter(memoryStream);
                await streamWriter.FlushAsync();
                exportFileName = $"exportedCsv-{DateTime.Now.ToFileTime()}.csv";
                exportUrl = await response.Content.ReadAsStringAsync();

            }

        }

    }
}
