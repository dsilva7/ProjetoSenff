using Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominios.Entities
{
    public class Sala
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public int Capacidade { get; set; }
        public string? RecursosJson { get; private set; }

        [NotMapped]
        public string[] RecursosValue
        {
            get
            {
                if (RecursosJson == null)
                {
                    return null;
                }
                else
                {
                    return JsonHelper.Deserialize<string[]>(RecursosJson);
                }
            }
            set
            {
                if (value == null)
                {
                    RecursosJson = null;
                }
                else
                {
                    RecursosJson = JsonHelper.Serialize(value);
                }
            }
        }
    }
}
