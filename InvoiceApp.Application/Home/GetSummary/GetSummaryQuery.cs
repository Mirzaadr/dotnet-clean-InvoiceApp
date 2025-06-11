using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Home.GetSummary;

public record GetSummaryQuery() : IRequest<SummaryDto>;