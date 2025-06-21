using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Alpha.Application.Interfaces;
using Alpha.Application.Products.Commands;
using Alpha.Application.Queries;
using Alpha.Domain.Interfaces;
using Alpha.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Api.Controllers
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Application.Queries.GetProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result); 
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID do corpo não corresponde ao da URL.");

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }

        [HttpPost("sync-fakestore")]
        public async Task<IActionResult> SyncFromFakeStore(
     [FromServices] IFakeStoreService fakeStoreService,
     [FromServices] IProductRepository repository)
        {
            try
            {
                var fakeProducts = await fakeStoreService.GetProductsFromFakeStoreAsync();
                int addedCount = 0;
                int updatedCount = 0;

                foreach (var p in fakeProducts)
                {
                    var barcode = $"789{p.Id.ToString().PadLeft(10, '0')}";
                    var existingProduct = await repository.GetByBarcodeAsync(barcode); 

                    if (existingProduct == null)
                    {
                        
                        Product newProduct = new Product
                        {
                            
                            Name = p.Title,
                            Price = (decimal)p.Price,
                            Barcode = barcode,
                            ImageBytes = p.Image,
                            CreatedAt = DateTime.UtcNow
                        };
                        await repository.AddAsync(newProduct);
                        addedCount++;
                    }
                    else
                    {
                        
                        existingProduct.Name = p.Title;
                        existingProduct.Price = (decimal)p.Price;
                        existingProduct.ImageBytes = p.Image;
                        await repository.UpdateAsync(existingProduct);
                        updatedCount++;
                    }
                }

                return Ok(new
                {
                    message = "Sincronização concluída com sucesso.",
                    added = addedCount,
                    updated = updatedCount,
                    totalProcessed = fakeProducts.Count()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Falha na sincronização",
                    details = ex.Message
                });
            }
        }


    }
}
