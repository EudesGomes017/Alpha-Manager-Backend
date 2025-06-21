using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Alpha.Application.DTOs
{
    public record ProductDto(int Id, string Name, decimal Price, string Barcode, string image );
}
