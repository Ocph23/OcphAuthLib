using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OcphAuthBlazorView
{
    public class ErrorResult
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status{ get; set; }
        public JsonElement Errors { get; set; }

        public Dictionary<string, string> GetErrors()
        {
            Dictionary<string, string> listError = new Dictionary<string, string>();

            try
            {
                var names = Errors.EnumerateObject().AsEnumerable();
                foreach (var item in names)
                {
                    var name = item.Name;
                    var property = item.Value.EnumerateArray().AsEnumerable();
                    foreach (var item2 in property)
                    {
                        var res = item2.GetString();
                        listError.Add(name, res);
                    }
                }

                return listError;
            }
            catch 
            {
                return listError;
            }
        }
    }

    public class Errors
    {
        public List<string> User { get; set; }
        public List<string> Password { get; set; }
    }
}
