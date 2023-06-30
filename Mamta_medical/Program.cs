// See https://aka.ms/new-console-template for more information
using Aspose.Cells;
using Dapper;
using Mamta_medical;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

class program
{
    static void Main(string[] args)
    {
        program p = new program();
        p.generate_product_sql();

    }
    public void csvtojson()
    {
        var workbook = new Workbook("D:\\Medicine_names\\Csv\\Allproducts.csv");
        /*  File.Create("D:\\VisualStudioProject\\sqltocsv\\sqltocsv\\mangal.json").Dispose();*/
        workbook.Save(@"D:\Projects\MamtaMedical\Json\Allproducts.json");

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
            var forms = Convert.ToString(jsondata.Packsize);
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

        foreach (FileInfo file in Files)
        {
            String[] delimiters = { "pdt", ".csv", "(1)", "1mg", "pdf" };
            String[] name = file.Name.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine(name[0]);

            var workbook = new Workbook(file.ToString());
            /*  File.Create("D:\\VisualStudioProject\\sqltocsv\\sqltocsv\\mangal.json").Dispose();*/
            workbook.Save("D:\\Projects\\MamtaMedical\\Json\\" + name[0] + ".json");
        }
    }

    public void add_brand_name()
    {
        DirectoryInfo d = new DirectoryInfo(@"D:\Projects\MamtaMedical\Json");
        FileInfo[] Files = d.GetFiles("*.json");
        /*  string[] filePaths = Directory.GetFiles(@"D:\Projects\MamtaMedical\Json", "*.json");*/
        foreach (var item in Files)
        {
            var data = File.ReadAllText(item.FullName.ToString());
            dynamic json = JsonConvert.DeserializeObject<List<dynamic>>(data);
            var name = item.Name.ToString().Split(".json");
            for (int j = 0; j < json.Count; j++)
            {
                json[j].Add("Brand_name", Camelcase(name[0].ToString()));
            }
            Console.WriteLine(json);
            var dd = JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText($@"D:\Projects\MamtaMedical\Json\Update\{name[0]}.json", dd);
        }


    }
    public string Camelcase(string str)
    {

        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
    }
    public void generate_brand()
    {
        List<dynamic> brand = new List<dynamic>();
        DirectoryInfo d = new DirectoryInfo(@"D:\Projects\MamtaMedical\Json");
        FileInfo[] Files = d.GetFiles("*.json");

        foreach (var item in Files)
        {
            var data = File.ReadAllText(item.FullName.ToString());
            dynamic json = JsonConvert.DeserializeObject<List<dynamic>>(data);
            for(int i = 0;i < json.Count; i++)
            {
                var jsondat = json[i].Brand_name.ToString().Split(" ",StringSplitOptions.RemoveEmptyEntries);
                var b = String.Join(" ", jsondat);
                brand.Add(b);
            }
            
        }
       var unique_brand =  brand.Distinct().ToArray();
        Console.WriteLine(unique_brand[0]);
        File.Create(@"D:\Projects\MamtaMedical\SQL\Brand.sql").Dispose();
        var women_sql = File.ReadAllLines("D:\\Projects\\MamtaMedical\\SQL\\Brand.sql").ToList();
        for (int i = 0; i < unique_brand.Length; i++)
        {
            var query = $"insert into brands(Brand_name, File_id)values('{Camelcase(unique_brand[i])}', '{get_unique_string(8)}')";
            women_sql.Insert(0, query);
            File.WriteAllLines("D:\\Projects\\MamtaMedical\\SQL\\Brand.sql", women_sql);
        }
    }
    public string get_unique_string(int string_length)
    {
        const string src = "ASDFGH012345JKLQWE012345RTYUIOPZXCVBNM6789";
        int length = 10;
        var sb = new StringBuilder();
        Random RNG = new Random();
        for (var i = 0; i < length; i++)
        {
            var c = src[RNG.Next(0, src.Length)];
            sb.Append(c);
        }
        return sb.ToString();
    }
    public string get_all_brand()
    {
        var cs = "Data Source=DESKTOP-TAB76U8\\BEAST;Initial Catalog=Mamta_medical_db; TrustServerCertificate=true; Integrated Security = True;";
        using var con = new SqlConnection(cs);
        con.Open();
        var cars = con.Query<dynamic>("SELECT * FROM brands").ToList();
        DirectoryInfo d = new DirectoryInfo(@"D:\Projects\MamtaMedical\Json");
        FileInfo[] Files = d.GetFiles("*.json");
        var dat = "hellos this is mangal singh";
        foreach (var item in Files)
        {
            var data = File.ReadAllText(item.FullName.ToString());
            dynamic json = JsonConvert.DeserializeObject<List<dynamic>>(data);
            for (int j = 0; j < json.Count; j++)
            {
                for (int i = 0; i < cars.Count; i++)
                {
                    var brand_name = json[j].Brand_name.ToString().Replace(" ", "").ToLower();
                    var brand = cars[i].Brand_name.ToString().Replace(" ", "").ToLower();
                    if(brand_name == brand)
                    {
                        json[j].Brand_id = cars[i].File_id;
                    }

                }
            
            }
            var dd = JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText($@"D:\Projects\MamtaMedical\Json\Update\{item.Name.ToString()}", dd);

        }
        return "bn";
    }
    public void generate_product_id()
    {
        DirectoryInfo d = new DirectoryInfo(@"D:\Projects\MamtaMedical\Json");
        FileInfo[] Files = d.GetFiles("*.json");
       
        foreach (var item in Files)
        {
            var data = File.ReadAllText(item.FullName.ToString());
            dynamic json = JsonConvert.DeserializeObject<List<dynamic>>(data);
            for (int i = 0; i < json.Count; i++)
            {
                json[i].Add("Product_id", get_unique_string(10));

            }
            var dd = JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText($@"D:\Projects\MamtaMedical\Json\Update\{item.Name.ToString()}", dd);
        }

    }
    public void generate_product_sql()
    {
        var u = 0;
        File.Create(@"D:\Projects\MamtaMedical\SQL\Products.sql").Dispose();
        var women_sql = File.ReadAllLines("D:\\Projects\\MamtaMedical\\SQL\\Products.sql").ToList();
        DirectoryInfo d = new DirectoryInfo(@"D:\Projects\MamtaMedical\Json");
        FileInfo[] Files = d.GetFiles("*.json");

        foreach (var item in Files)
        {
            var data = File.ReadAllText(item.FullName.ToString());
            dynamic json = JsonConvert.DeserializeObject<List<dynamic>>(data);
            for (int i = 0; i < json.Count; i++)
            {
               
                string result = Regex.Replace(json[i]s.Product_name.ToString(), @"'", @"''");
                var query = @$"insert into trial_products(Product_name,Product_id,Brand_id)values('{result}','{json[i].Product_id.ToString()}','{json[i].Brand_id.ToString()}')";
                women_sql.Insert(0, query);
                u++;
                Console.WriteLine(u);
            }
        }
        File.WriteAllLines("D:\\Projects\\MamtaMedical\\SQL\\Products.sql",Enumerable.Reverse(women_sql));


    }
}