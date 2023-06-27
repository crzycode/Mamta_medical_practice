// See https://aka.ms/new-console-template for more information
using Aspose.Cells;
using Newtonsoft.Json;
using System.Xml;

class program
{
    static void Main(string[] args)
    {
        program p = new program();
       p.multiple_csv_to_json();

    }
    public void csvtojson()
    {



        var workbook = new Workbook("D:\\Medical\\flipProduct.csv");
        /*  File.Create("D:\\VisualStudioProject\\sqltocsv\\sqltocsv\\mangal.json").Dispose();*/
        workbook.Save("D:\\Medical\\flipProduct.json");

        /*string path = "D:\\Downloads\\ajio_data.csv";
        var csv = new List<string[]>();
        var lines = File.ReadAllLines(path);

        foreach (string line in lines)
            csv.Add(line.Split(','));

        var properties = lines[0].Split(',');

        var listObjResult = new List<Dictionary<string, string>>();

        for (int i = 1; i < lines.Length; i++)
        {
            var objResult = new Dictionary<string, string>();
            for (int j = 0; j < properties.Length; j++)
                objResult.Add(properties[j], csv[i][j]);

            listObjResult.Add(objResult);
        }
        File.Create("D:\\VisualStudioProject\\sqltocsv\\sqltocsv\\mangal.json").Dispose();

       
        var json = JsonConvert.SerializeObject(listObjResult, Formatting.Indented);

       

        File.WriteAllText(@"D:\Data\mangal.json", json);
*/

    }
    public void single_json()
    {
        var data = File.ReadAllText("D:\\Medical\\flipProduct.json");

        int j = 0;
        dynamic json = JsonConvert.DeserializeObject<List<dynamic>>(data);

        for (int i = 0; i < json.Count; i++)
        {
            var jsondat = JsonConvert.SerializeObject(json[i], Newtonsoft.Json.Formatting.Indented);
            var jsondata = JsonConvert.DeserializeObject<dynamic>(jsondat);
           var forms =  Convert.ToString(jsondata.Packsize);
            string[] discription = forms.Split(" ");
            var formtype = discription.Length - 1;
            jsondata.Add("URL_image", $"{jsondata.Product_id}.jpg");
            jsondata.Add("Form", discription[formtype]);    

            var d = JsonConvert.SerializeObject(jsondata, Newtonsoft.Json.Formatting.Indented);
           
            File.Create($@"D:\Medical\Medicine_data\MamtaMedical_{jsondata.Product_id}.json").Dispose();
            File.WriteAllText($@"D:\Medical\Medicine_data\MamtaMedical_{jsondata.Product_id}.json", d);

                Console.WriteLine($"{jsondata.id}");
            
        }
    }

    public void multiple_csv_to_json()
    {
        DirectoryInfo d = new DirectoryInfo(@"D:\Medicine_names\Csv");
        FileInfo[] Files = d.GetFiles("*.csv"); //Getting Text files

        foreach ( FileInfo file in Files )
        {
            String[] delimiters = { "pdt", ".csv","(1)", "1mg","pdf" };
            String[] name = file.Name.Split(delimiters, StringSplitOptions.None);
            
                Console.WriteLine(name[0]);

            var workbook = new Workbook(file.ToString());
            /*  File.Create("D:\\VisualStudioProject\\sqltocsv\\sqltocsv\\mangal.json").Dispose();*/
            workbook.Save("D:\\Projects\\MamtaMedical\\Json\\"+ name[0]+".json");


        }

        string str = "";
    }
}