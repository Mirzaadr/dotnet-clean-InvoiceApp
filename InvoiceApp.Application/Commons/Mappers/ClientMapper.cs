using InvoiceApp.Application.DTOs;
using InvoiceApp.Domain.Clients;

namespace InvoiceApp.Application.Commons.Mappers;

public static class ClientMapper
{
  public static ClientDto ToDto(Client client)
  {
    return new ClientDto
    {
      Id = client.Id.Value,
      Name = client.Name,
      Address = client.Address,
      Email = client.Email,
      PhoneNumber = client.PhoneNumber,
      CreatedDate = client.CreatedDate,
      UpdatedDate = client.UpdatedDate,
    };
  }
}