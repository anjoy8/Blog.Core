using System.Linq;
using System.Threading.Tasks;

namespace Blog.Core.Model.ViewModels
{
    public class EnumDemoDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Type Description balabala
        /// </summary>
        public EnumType Type { get; set; }
    }


    public enum EnumType
    {
        foo, bar, baz
    }
}
