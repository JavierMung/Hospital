namespace API.ViewModels
{
	public record ViewTicketAdd(int IdTrabajador, List<ViewInsumoTicket> Insumos, List<ViewServicioTicket> Servicios);
	public record ViewTicketResponse(ViewTicketAdd Respuesta, decimal Total, int IdTicket);
}
