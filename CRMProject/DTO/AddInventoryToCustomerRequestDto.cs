using CRMProject.Models;
using System.ComponentModel.DataAnnotations;

namespace CRMProject.DTO;

public class AddInventoryToCustomerRequestDto
{
    public int CustomerId { get; set; }
}
